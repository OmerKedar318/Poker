import json
import socket
import select

HOST = "0.0.0.0"
PORT = 5555

server = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
server.setsockopt(socket.SOL_SOCKET, socket.SO_REUSEADDR, 1)
server.bind((HOST, PORT))
server.listen()

sockets_list = [server]
clients = {}

game_state = {
    "players": [],
    "pot": 0,
    "current_turn": 0,  # Index in players list
    "highest_bet": 0,   # Current bet to call
    "bets": {}
}


def receive_message(client_socket):
    try:
        data = client_socket.recv(1024).decode()
        if not data:
            return None
        return json.loads(data)
    except:
        return None


def broadcast(message):
    data = json.dumps(message).encode()
    for client_socket in clients:
        client_socket.send(data)


print(f"Server listening on {HOST}:{PORT}")

while True:
    read_sockets, _, exception_sockets = select.select(sockets_list, [], sockets_list)
    for notified in read_sockets:
        if notified == server:
            client_socket, client_address = server.accept()
            sockets_list.append(client_socket)
            clients[client_socket] = client_address
            game_state["players"].append(client_socket)
            print("New player connected " + str(client_address))
            broadcast({"action": "update", "message": "New player connected" + str(client_address)})
        else:
            message = receive_message(notified)
            if message is None:
                print("Player " + str(clients[notified]) + " disconnected")
                sockets_list.remove(notified)
                del clients[notified]
                game_state["players"].remove(notified)
                continue
            action = message["action"]
            if action == "bet":
                amount = message["amount"]
                game_state["bets"][notified] = amount
                game_state["pot"] += amount
                print("Player bet " + amount)
                if amount > game_state["highest_bet"]:
                    game_state["highest_bet"] = amount
            elif action == "fold":
                print("Player folded")
                game_state["players"].remove(notified)
            game_state["current_turn"] = (game_state["current_turn"] + 1) % len(game_state["players"])
            next_player = game_state["players"][game_state["current_turn"]]
            broadcast({
                "action": "update",
                "pot": game_state["pot"],
                "current_turn": clients[next_player],
                "highest_bet": game_state["highest_bet"]
            })
    for notified in exception_sockets:
        sockets_list.remove(notified)
        del clients[notified]
