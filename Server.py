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
            print("New player connected " + client_address)
        else:
            message = receive_message(notified)
            if message is None:
                print("Player " + clients[notified] + " disconnected")
                sockets_list.remove(notified)
                del clients[notified]
                continue
            action = message["action"]
            if action == "bet":
                amount = message["amount"]
                print("Player bet " + amount)
                broadcast({"action": "update", "pot": amount})
            elif action == "fold":
                print("Player folded")
                broadcast({"action": "update", "player_folded": True})
    for notified in exception_sockets:
        sockets_list.remove(notified)
        del clients[notified]
