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

type Tables struct {
	Tables []interface{} `json:"tables"`
}

func (ts *Tables) append(name string) error {
	table, err := createTable(name)
	ts.Tables = append(ts.Tables, table)
	return err
}

func (ts *Tables) appendFew(names []string) error {
	var err error
	for _, name := range names {
		err = ts.append(name)
	}
	return err
}

type Hostel struct {
	Items []HostelItem `json:"hostel"`
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
	Items []PhoneItem `json:"phone"`
}

type PhoneItem struct {
	IDdPhone uint   `db:"id_phone" json:"id_phone"`
	Phone    string `db:"phone" json:"phone"`
}

type Hostel2Metro struct {
	Items []Hostel2MetroItem `json:"hostel2metro"`
}

type Hostel2MetroItem struct {
	IDHostel      uint `db:"id_hostel" json:"id_hostel"`
	IDHoste2Metro uint `db:"id_hostel2metro" json:"id_hostel2metro"`
	IDdMetro      uint `db:"id_metro" json:"id_metro"`
}

type Hostel2Phone struct {
	Items []Hostel2PhoneItem `json:"hostel2phone"`
}

type Hostel2PhoneItem struct {
	IDHostel      uint `db:"id_hostel" json:"id_hostel"`
	IDHoste2Phone uint `db:"id_hostel2phone" json:"id_hostel2phone"`
	IDPhone       uint `db:"id_phone" json:"id_phone"`
}

type Metro struct {
	Items []MetroItem `json:"metro"`
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

func getProtatype(name string) interface{} {
	tableMap := map[string]interface{}{
		"hostel":       Hostel{},
		"phone":        Phone{},
		"hostel2metro": Hostel2Metro{},
		"hostel2phone": Hostel2Phone{},
		"metro":        Metro{}}

	return tableMap[name]
}

func createTable(name string) (interface{}, error) {
	rows, err := db.Queryx(fmt.Sprintf("SELECT * FROM %s", name))
	if err != nil {
		return nil, err
	}

	result := reflect.New(reflect.TypeOf(getProtatype(name))).Elem()
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
	tables.appendFew([]string{"hostel", "phone", "hostel2metro", "hostel2phone", "metro"})

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
