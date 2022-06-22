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
	http.HandleFunc("/mongo/getAll", service.ConnectToMongoDB)
	http.ListenAndServe(fmt.Sprintf(":$s", port), nil)
}
