package main

import (
	"log"
	"net/http"

	"unityservertictactoe/handlers"
)

func main() {
	http.HandleFunc("/ws", handlers.WSHandler)
	log.Println("WebSocket server running on :8080")
	log.Fatal(http.ListenAndServe(":8080", nil))
}
