package main

import (
	"fmt"
	"io"
	"net/http"

	"user/hostel/utils"
)

const (
	KeyVersion = "ver"
	Udid       = "udid"
	HostelID   = "id_hostel"
	Action     = "action"
)

var (
	exit = make(chan int)
)

func version(w http.ResponseWriter, r *http.Request) {
	versionDB, err := utils.GetCurrentVersionDB()
	if err != nil {
		utils.Error(err.Error())
		http.Error(w, err.Error(), http.StatusInternalServerError)
	} else {
		io.WriteString(w, versionDB)
	}
}

func db(w http.ResponseWriter, r *http.Request) {
	version := r.URL.Query().Get(KeyVersion)
	versionDB, err := utils.GetCurrentVersionDB()
	if err != nil {
		utils.Error(err.Error())
	}
	utils.Info(fmt.Sprintf("Ver DB => req: [%s], cur: [%s]\n", version, versionDB))
	if version != versionDB {
		str, err := utils.GetHostelDB(version)
		if err != nil {
			utils.Error(err.Error())
			http.Error(w, err.Error(), http.StatusInternalServerError)
		} else {
			io.WriteString(w, str)
		}
	}
}

func register(w http.ResponseWriter, r *http.Request) {
	udid := r.URL.Query().Get(Udid)
	fmt.Printf("[I] Register new user: %s\n", udid)
	err := utils.Register(udid)
	if err != nil {
		utils.Error(err.Error())
		http.Error(w, err.Error(), http.StatusInternalServerError)
	} else {
		io.WriteString(w, "registered")
	}
}

func clientAction(w http.ResponseWriter, r *http.Request) {
	udid := r.URL.Query().Get(Udid)
	action := r.URL.Query().Get(Action)
	hostelID := r.URL.Query().Get(HostelID)
	if udid == "" {
		utils.Error("udid empty")
		http.Error(w, "udid empty", http.StatusBadRequest)
		return
	}
	utils.Info(fmt.Sprintf("udid: %s, hostel: %s action: %s", udid, hostelID, action))
	err := utils.ClientAction(udid, hostelID, action)
	if err != nil {
		utils.Error(err.Error())
		http.Error(w, err.Error(), http.StatusInternalServerError)
	}
}

func serve() {
	utils.OpenDB()
	defer utils.CloseDB()
	http.HandleFunc("/system/version", version)
	http.HandleFunc("/system/db", db)
	http.HandleFunc("/client/register", register)
	http.HandleFunc("/client/action", clientAction)
	http.HandleFunc("/exit", func(_ http.ResponseWriter, _ *http.Request) { //remove from release (RFR)
		exit <- 1
	})

	http.ListenAndServe(":8000", nil)
}

func main() {
	go serve()
	fmt.Printf("[I] %s\n", "Fun sImpLe quIck :)")
	<-exit
}
