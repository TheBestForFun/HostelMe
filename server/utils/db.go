package utils

import (
	"encoding/json"
	"errors"
	"fmt"
	"reflect"

	_ "github.com/go-sql-driver/mysql" //driver db
	"github.com/jmoiron/sqlx"
)

var (
	db *sqlx.DB
)

const (
	dbDriver    = "mysql"
	dbLoginPass = "user:user@/Hostel"

	tableVersion  = "version"
	tableUserTest = "user_test"

	fieldDbVersion         = "db_version"
	fieldHostelDateUpdate  = "h_date_update"
	fieldVersionDateUpdate = "date_update"

	flagRegistrated = 0

	dbReq           = "SELECT %s FROM %s"
	dbReqWhere      = " where %s > '%s'"
	dbRewGetVerDate = "SELECT %s FROM %s WHERE %s = '%s';"
	dbRewGetVer     = "SELECT %s FROM %s ORDER BY %s DESC LIMIT 1;"

	dbReqRegisteration = "INSERT INTO %s (udid, result) VALUES('%s', '%d');"
	dbReqAction        = "INSERT INTO %s (udid, result, id_hostel) VALUES('%s', '%s', '%s');"

	tagSQLFields  = "sql"
	tagPrimaryKey = "primary"
	tagTable      = "table"
)

// Exported tabels from DB
// primary tag - primary index to exclude equal record
// sql tag - fields to export
type Tables struct {
	Hostels       []Hostel       `json:"hostels, omitempty" primary:"IDHostel" table:"hostel_view" sql:"id_hostel, address, h_name, site, h_latitude, h_longitude, h_date_add, h_date_update"`
	Phones        []Phone        `json:"phones, omitempty" primary:"IDPhone" table:"hostel_view" sql:"id_phone, phone"`
	Metros        []Metro        `json:"metros, omitempty" primary:"IDMetro" table:"hostel_view" sql:"id_metro, m_name, m_longitude, m_latitude"`
	Hostel2Metros []Hostel2Metro `json:"hostel2metros, omitempty" primary:"IDHoste2Metro" table:"hostel_view" sql:"id_hostel2metro, id_hostel, id_metro"`
	Hostel2Phones []Hostel2Phone `json:"hostel2phones, omitempty" primary:"IDHoste2Phone" table:"hostel_view" sql:"id_hostel2phone, id_hostel, id_phone"`
	Versions      []Version      `json:"versions, omitempty" primary:"ID" table:"version" sql:"id, db_version, date_update"`
}

type Hostel struct {
	Address    string  `db:"address" json:"address"`
	DateAdd    string  `db:"h_date_add" json:"h_date_add"`
	DateUpdate string  `db:"h_date_update" json:"h_date_update"`
	IDHostel   uint    `db:"id_hostel" json:"id_hostel"`
	Latitude   float64 `db:"h_latitude" json:"h_latitude"`
	Longitude  float64 `db:"h_longitude" json:"h_longitude"`
	Name       string  `db:"h_name" json:"h_name"`
	Site       string  `db:"site" json:"site"`
}

type Phone struct {
	IDPhone uint   `db:"id_phone" json:"id_phone"`
	Phone   string `db:"phone" json:"phone"`
}

type Hostel2Metro struct {
	IDHostel      uint `db:"id_hostel" json:"id_hostel"`
	IDHoste2Metro uint `db:"id_hostel2metro" json:"id_hostel2metro"`
	IDdMetro      uint `db:"id_metro" json:"id_metro"`
}

type Hostel2Phone struct {
	IDHostel      uint `db:"id_hostel" json:"id_hostel"`
	IDHoste2Phone uint `db:"id_hostel2phone" json:"id_hostel2phone"`
	IDPhone       uint `db:"id_phone" json:"id_phone"`
}

type Metro struct {
	IDMetro   uint    `db:"id_metro" json:"id_metro"`
	Latitude  float64 `db:"m_latitude" json:"m_latitude"`
	Longitude float64 `db:"m_longitude" json:"m_longitude"`
	Name      string  `db:"m_name" json:"m_name"`
}

type Version struct {
	ID         uint   `db:"id" json:"id"`
	DBVersion  string `db:"db_version" json:"db_version"`
	DateUpdate string `db:"date_update" json:"date_update"`
}

type TableRow map[string]string

func (ts *Tables) fill(ver string) error {
	tables := reflect.ValueOf(ts).Elem()
	for i := 0; i < tables.NumField(); i++ {
		table, err := createTable(tables.Field(i), tables.Type().Field(i), ver)
		if err != nil {
			return err
		} else {
			tables.Field(i).Set(reflect.ValueOf(table))
		}
	}
	return nil
}

func contains(s []uint64, e uint64) bool {
	for _, a := range s {
		if a == e {
			return true
		}
	}
	return false
}

func createTable(tableValue reflect.Value, tableType reflect.StructField, ver string) (interface{}, error) {
	var where string

	date, err := GetVersionDate(ver)

	if date != "" {
		field := fieldHostelDateUpdate
		if tableType.Tag.Get(tagTable) == tableVersion {
			field = fieldVersionDateUpdate
		}
		where = fmt.Sprintf(dbReqWhere, field, date)
	}

	sql := fmt.Sprintf(dbReq, tableType.Tag.Get(tagSQLFields), tableType.Tag.Get(tagTable))

	rows, err := db.Queryx(sql + where)
	if err != nil {
		return nil, err
	}

	result := reflect.New(tableValue.Type()).Elem()
	rowItem := reflect.New(tableValue.Type().Elem()).Interface()
	primaryKeys := make([]uint64, 10)
	for rows.Next() {
		err = rows.StructScan(rowItem)
		if err != nil {
			return nil, err
		}
		// not include dublicate from view
		key := reflect.ValueOf(rowItem).Elem().FieldByName(tableType.Tag.Get(tagPrimaryKey))
		if contains(primaryKeys, key.Uint()) == false {
			primaryKeys = append(primaryKeys, key.Uint())
			result.Set(reflect.Append(result, reflect.ValueOf(rowItem).Elem()))
		}
	}
	return result.Interface(), nil
}

func OpenDB() error {
	var err error
	db, err = sqlx.Open(dbDriver, dbLoginPass)
	if err != nil {
		return err
	}

	err = db.Ping()
	if err != nil {
		return err
	}
	return nil
}

func CloseDB() {
	db.Close()
}

func GetHostelDB(ver string) (string, error) {

	var tables Tables
	err := tables.fill(ver)
	if err != nil {
		return "", err
	}
	jsonData, err := json.Marshal(tables)
	if err != nil {
		return "", err
	}
	return string(jsonData), nil
}

func GetVersionDate(ver string) (string, error) {
	if ver == "" {
		return "", errors.New("version is empty")
	}

	rows, err := db.Query(fmt.Sprintf(dbRewGetVerDate, fieldVersionDateUpdate, tableVersion, fieldDbVersion, ver))
	if err != nil {
		return "", err
	}

	rows.Next()
	var date string
	err = rows.Scan(&date)
	if err != nil {
		return "", err
	}
	return date, nil
}

func GetCurrentVersionDB() (string, error) {
	rows, err := db.Query(fmt.Sprintf(dbRewGetVer, fieldDbVersion, tableVersion, fieldVersionDateUpdate))
	if err != nil {
		return "", err
	}

	rows.Next()
	var ver string
	err = rows.Scan(&ver)
	if err != nil {
		return "", err
	}
	return ver, nil

}
func Register(udid string) error {
	_sql := fmt.Sprintf(dbReqRegisteration, tableUserTest, udid, flagRegistrated)
	_, err := db.Exec(_sql)
	if err != nil {
		return err
	}
	return nil
}
func ClientAction(udid, hostel, action string) error {
	_sql := fmt.Sprintf(dbReqAction, tableUserTest, udid, action, hostel)
	_, err := db.Exec(_sql)
	if err != nil {
		return err
	}
	return nil
}
