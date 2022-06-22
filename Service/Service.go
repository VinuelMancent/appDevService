package Service

import (
	"context"
	"encoding/json"
	"fmt"
	"go.mongodb.org/mongo-driver/bson"
	"go.mongodb.org/mongo-driver/mongo"
	"go.mongodb.org/mongo-driver/mongo/options"
	"log"
	"net/http"
	"time"
)

type Tutorial struct {
	Id    int            `bson:"id,omitempty" json:"id"`
	Title string         `bson:"title,omitempty" json:"title"`
	Steps []TutorialStep `bson:"steps,omitempty" json:"steps""`
}

type TutorialStep struct {
	Id      int    `bson:"id,omitempty" json:"id"`
	Content string `bson:"content,omitempty" json:"content"`
}

type Service struct {
}

func (s *Service) ConnectToMongoDB(w http.ResponseWriter, req *http.Request) {
	var username = "AppDev"
	var passwd = "LfD9XvAMDWJRPqwE"
	var mongoURL = fmt.Sprintf("mongodb+srv://%s:%s@tutorialapp.bvdap.mongodb.net/?retryWrites=true&w=majority", username, passwd)
	client, err := mongo.NewClient(options.Client().ApplyURI(mongoURL))
	if err != nil {
		log.Fatal(err)
	}
	ctx, _ := context.WithTimeout(context.Background(), 10*time.Second)
	err = client.Connect(ctx)
	if err != nil {
		log.Fatal(err)
	}
	defer client.Disconnect(ctx)

	var databaseName = "Tutorials"
	var collectionName = "Tutorials"

	var collection = client.Database(databaseName).Collection(collectionName)
	findOptions := options.Find()
	var result, error = collection.Find(context.TODO(), bson.D{{}}, findOptions)
	if error != nil {
		log.Fatal(error.Error())
	}
	var results []Tutorial
	for result.Next(context.TODO()) {
		//Create a value into which the single document can be decoded
		var elem Tutorial
		err := result.Decode(&elem)
		if err != nil {
			log.Fatal(err)
		}

		results = append(results, elem)

	}
	fmt.Println(results)

	b, err := json.Marshal(results)
	if err != nil {
		fmt.Println(err)
	}
	w.Write(b)
}
