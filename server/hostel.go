package main

import (
	"fmt"
	"io"
	"net/http"

	"user/hostel/utils"
)

const (
	keyVersion = "ver"
	UDID       = "udid"
	hostel_id  = "id_hostel"
	action     = "action"
)

var (
	exit = make(chan int)
)

func checkVersion(w http.ResponseWriter, r *http.Request) {
	version := r.URL.Query().Get(keyVersion)
	versionDB := utils.GetHostelVersion()
	fmt.Printf("Ver request: %s, DB: %s\n", version, versionDB)
	if version != versionDB {
		str, err := utils.GetHostelDB()
		if err != nil {
			fmt.Println(err.Error())
		}
		io.WriteString(w, str)
	}
}

func register(w http.ResponseWriter, r *http.Request) {
	udid := r.URL.Query().Get(UDID)
	fmt.Printf("Register new user: %s\n", udid)
	err := utils.Register(udid)
	if err != nil {
		fmt.Println(err.Error())
	}
	io.WriteString(w, "register")
}

func hostelAction(w http.ResponseWriter, r *http.Request) {
	udid := r.URL.Query().Get(UDID)
	_action := r.URL.Query().Get(action)
	_hostel_id := r.URL.Query().Get(hostel_id)
	fmt.Printf("udid: %s, hostel: %s action: %s", udid, _hostel_id, _action)
	err := utils.HostelAction(udid, _hostel_id, _action)
	if err != nil {
		fmt.Println(err.Error())
	}
}

func serve() {
	utils.OpenDB()
	defer utils.CloseDB()
	http.HandleFunc("/system/checkVersion", checkVersion)
	http.HandleFunc("/user/register", register)
	http.HandleFunc("/user/action", hostelAction)
	http.HandleFunc("/exit", func(_ http.ResponseWriter, _ *http.Request) {
		exit <- 1
	})

	http.ListenAndServe(":8000", nil)
}

func main() {
	go serve()
	fmt.Println("wait")
	<-exit
}
