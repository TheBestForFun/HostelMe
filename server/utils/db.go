package utils

import (
	"database/sql"
	"encoding/json"
	"fmt"

	_ "github.com/go-sql-driver/mysql"
)

var (
	db *sql.DB
)

const (
	table_settings  = "settings"
	table_user_test = "user_test"

	db_version       = "db_version"
	last_update_date = "last_update_date"

	res_registr = 0
)

type Tables struct {
	Tables []Table `json:"tables"`
}

func (ts *Tables) append(name string) error {
	table, err := getTable(name)
	ts.Tables = append(ts.Tables, *table)
	return err
}

func (ts *Tables) appendFew(names []string) error {
	var err error
	for _, name := range names {
		err = ts.append(name)
	}
	return err
}

type Table struct {
	Name string     `json:"name"`
	Rows []TableRow `json:"rows"`
}

func (t *Table) append(row TableRow) {
	t.Rows = append(t.Rows, row)
}

type TableRow map[string]string

func getTable(name string) (*Table, error) {
	rows, err := db.Query(fmt.Sprintf("SELECT * FROM %s", name))
	if err != nil {
		return nil, err
	}

	columns, err := rows.Columns()
	if err != nil {
		return nil, err
	}

	table := Table{Name: name, Rows: []TableRow{}}
	valuesPtr := make([]interface{}, len(columns))
	values := make([]string, len(columns))

	for i := range values {
		valuesPtr[i] = &values[i]
	}

	for rows.Next() {
		rows.Scan(valuesPtr...)
		var row TableRow
		row = make(map[string]string)

		for i, value := range values {
			row[columns[i]] = value
		}
		table.append(row)
	}
	return &table, nil
}

func OpenDB() error {
	var err error
	db, err = sql.Open("mysql", "user:user@/Hostel")
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
	tables.appendFew([]string{"hostel", "phone", "hostel2metro", "hostel2phone", "metro", "result"})

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
