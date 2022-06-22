package main

import (
	"net/http"
	"tutorialAppService/Service"
)

func main() {
	service := Service.Service{}
	//service.ConnectToMongoDB()

	http.HandleFunc("/mongo/getAll", service.ConnectToMongoDB)
	http.ListenAndServe(":8080", nil)
}
