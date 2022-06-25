package main

import (
	"fmt"
	"net/http"
	"os"
	"tutorialAppService/Service"
)

func main() {
	service := Service.Service{}
	//service.ConnectToMongoDB()

	port := os.Getenv("PORT")
	if port == "" {
		port = "8080"
	}
	http.HandleFunc("/mongo/getAll", service.GetAll)
	http.HandleFunc("/mongo/Add", service.AddTutorial)
	http.ListenAndServe(fmt.Sprintf(":%s", port), nil)
}
