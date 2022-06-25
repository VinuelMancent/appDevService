package Service

import (
	"context"
	"encoding/json"
	"fmt"
	"go.mongodb.org/mongo-driver/bson"
	"go.mongodb.org/mongo-driver/mongo"
	"go.mongodb.org/mongo-driver/mongo/options"
	"log"
	"math/rand"
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
	client        *mongo.Client
	clientContext context.Context
	collection    *mongo.Collection
}

func (s *Service) GetAll(w http.ResponseWriter, req *http.Request) {
	s.init()
	defer s.client.Disconnect(s.clientContext)
	fmt.Println("finished init")
	findOptions := options.Find()
	var result, error = s.collection.Find(context.TODO(), bson.D{{}}, findOptions)
	if error != nil {
		log.Fatal(error.Error())
	}
	fmt.Println("loaded all resources from db")
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
	fmt.Println("got all results as results")
	fmt.Println(results)

	b, err := json.Marshal(results)
	if err != nil {
		fmt.Println(err)
	}
	w.Write(b)
}

func (s *Service) AddTutorial(w http.ResponseWriter, req *http.Request) {
	fmt.Print("Content Type:")
	fmt.Println(req.Header.Get("Content-Type"))
	if req.Method != http.MethodPost {
		fmt.Println("only accepting POST")
		return
	}
	s.init()
	defer s.client.Disconnect(s.clientContext)
	var tutorial Tutorial
	var tutorialAsBytes []byte
	insertOptions := options.InsertOne()
	err := json.NewDecoder(req.Body).Decode(&tutorial)
	if err != nil {
		http.Error(w, err.Error(), http.StatusBadRequest)
		return
	}
	/*req.Body.Read(tutorialAsBytes)
	fmt.Println(tutorialAsBytes)
	fmt.Println(string(tutorialAsBytes))
	json.Unmarshal(tutorialAsBytes, &tutorial)*/
	if tutorial.Id == 0 {
		tutorial.Id = s.generateID()
	}
	if tutorial.Title == "" {
		return
	}
	_, err = s.collection.InsertOne(context.TODO(), tutorial, insertOptions)
	if err != nil {
		fmt.Println(err.Error())
		return
	}
	fmt.Printf("added %s", tutorial.Title)
	w.Write([]byte("success"))
}

func (s *Service) init() {
	var username = "AppDev"
	var passwd = "LfD9XvAMDWJRPqwE"
	var mongoURL = fmt.Sprintf("mongodb+srv://%s:%s@tutorialapp.bvdap.mongodb.net/?retryWrites=true&w=majority", username, passwd)
	s.client, _ = mongo.NewClient(options.Client().ApplyURI(mongoURL))

	s.clientContext, _ = context.WithTimeout(context.Background(), 10*time.Second)
	err := s.client.Connect(s.clientContext)
	if err != nil {
		log.Fatal(err)
	}

	var databaseName = "Tutorials"
	var collectionName = "Tutorials"

	s.collection = s.client.Database(databaseName).Collection(collectionName)
}

func (s *Service) generateID() int {
	number := rand.Intn(99999)
	if number < 10000 {
		return s.generateID()
	}
	return number
}
