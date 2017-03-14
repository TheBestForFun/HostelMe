package utils

import (
	"encoding/json"
	"fmt"

	"reflect"

	"github.com/jmoiron/sqlx"

	_ "github.com/go-sql-driver/mysql"
)

var (
	db *sqlx.DB
)

const (
	table_settings  = "settings"
	table_user_test = "user_test"

	db_version       = "db_version"
	last_update_date = "last_update_date"

	res_registr = 0
)

type Tables struct { // here json and table name a the same
	Hostel       Hostel       `json:"hostel"`
	Phone        Phone        `json:"phone"`
	Metro        Metro        `json:"metro"`
	Hostel2Metro Hostel2Metro `json:"hostel2metro"`
	Hostel2Phone Hostel2Phone `json:"hostel2phone"`
}

func (ts *Tables) fill() error {
	tables := reflect.ValueOf(ts).Elem()
	for i := 0; i < tables.NumField(); i++ {
		table, err := createTable(tables.Field(i), tables.Type().Field(i))
		if err != nil {
			panic(err)
		}
		tables.Field(i).Set(reflect.ValueOf(table))
	}
	return nil
}

type Hostel struct {
	Items []HostelItem `json:"hostelItems"`
}

type HostelItem struct {
	Address    string  `db:"address" json:"address"`
	DateAdd    string  `db:"date_add" json:"date_add"`
	DateUpdate string  `db:"date_update" json:"date_update"`
	IDdHostel  uint    `db:"id_hostel" json:"id_hostel"`
	Latitude   float64 `db:"latitude" json:"latitude"`
	Longitude  float64 `db:"longitude" json:"longitude"`
	Name       string  `db:"name" json:"name"`
	Site       string  `db:"site" json:"site"`
}

type Phone struct {
	Items []PhoneItem `json:"phoneItems"`
}

type PhoneItem struct {
	IDdPhone uint   `db:"id_phone" json:"id_phone"`
	Phone    string `db:"phone" json:"phone"`
}

type Hostel2Metro struct {
	Items []Hostel2MetroItem `json:"hostel2metroItems"`
}

type Hostel2MetroItem struct {
	IDHostel      uint `db:"id_hostel" json:"id_hostel"`
	IDHoste2Metro uint `db:"id_hostel2metro" json:"id_hostel2metro"`
	IDdMetro      uint `db:"id_metro" json:"id_metro"`
}

type Hostel2Phone struct {
	Items []Hostel2PhoneItem `json:"hostel2phoneItems"`
}

type Hostel2PhoneItem struct {
	IDHostel      uint `db:"id_hostel" json:"id_hostel"`
	IDHoste2Phone uint `db:"id_hostel2phone" json:"id_hostel2phone"`
	IDPhone       uint `db:"id_phone" json:"id_phone"`
}

type Metro struct {
	Items []MetroItem `json:"metroItems"`
}

type MetroItem struct {
	IDMetro   uint    `db:"id_metro" json:"id_metro"`
	Latitude  float64 `db:"latitude" json:"latitude"`
	Longitude float64 `db:"longitude" json:"longitude"`
	Name      string  `db:"name" json:"name"`
}

type TableRow map[string]string

func Parse(table interface{}, data map[string]string) {
	mapB, _ := json.Marshal(data)
	if err := json.Unmarshal(mapB, &table); err != nil {
		panic(err)
	}
}

func createTable(tableValue reflect.Value, tableType reflect.StructField) (interface{}, error) {
	rows, err := db.Queryx(fmt.Sprintf("SELECT * FROM %s", tableType.Tag.Get("json")))
	if err != nil {
		return nil, err
	}

	result := reflect.New(tableValue.Type()).Elem()
	s := result.FieldByName("Items")

	rowItem := reflect.New(s.Type().Elem()).Interface()

	for rows.Next() {
		err = rows.StructScan(rowItem)
		if err != nil {
			panic(err)
		}

		s.Set(reflect.Append(s, reflect.ValueOf(rowItem).Elem()))
	}
	return result.Interface(), nil
}

func OpenDB() error {
	var err error
	db, err = sqlx.Open("mysql", "user:user@/Hostel")
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

func GetHostelDB() (string, error) {

	var tables Tables
	tables.fill()

	jsonData, err := json.Marshal(tables)
	if err != nil {
		return "", err
	}
	return string(jsonData), nil
}

func GetHostelVersion() string {
	rows, err := db.Query(fmt.Sprintf("SELECT %s FROM %s ORDER BY %s DESC LIMIT 1;", db_version, table_settings, last_update_date))
	if err != nil {
		fmt.Println(err.Error())
		return ""
	}

	rows.Next()
	var ver string
	err = rows.Scan(&ver)
	if err != nil {
		fmt.Println(err.Error())
		return ""
	}
	return ver

}
func Register(udid string) error {
	_sql := fmt.Sprintf("INSERT INTO %s (udid, result) VALUES('%s', '%d');", table_user_test, udid, res_registr)
	fmt.Println(_sql)
	_, err := db.Exec(_sql)
	if err != nil {
		fmt.Println(err.Error())
	}
	return nil
}
func HostelAction(udid, hostel, action string) error {
	_sql := fmt.Sprintf("INSERT INTO %s (udid, result, id_hostel) VALUES('%s', '%s', '%s');", table_user_test, udid, action, hostel)
	fmt.Println(_sql)
	_, err := db.Exec(_sql)
	if err != nil {
		fmt.Println(err.Error())
	}
	return nil
}
