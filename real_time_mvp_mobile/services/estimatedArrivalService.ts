import { Buffer } from "buffer";

type Location = {
    longitude: string,
    latitude: string,
    distance: string
}

export default class EstimatedArrivalService {
    #socket = new WebSocket("ws://localhost:5124/ws");

    sendLocation(location: { latitude: number, longitude: number }, callback: (location: Location) => void) {
        if (this.#socket.readyState === this.#socket.OPEN) {
            const message = `${location.latitude};${location.longitude}`;
            const binaryData = Buffer.from(message, "utf-8");
            this.#socket.send(binaryData);

            const messageHandler = (event: MessageEvent) => {
                if (!event.data) return;
                const [lat, long, distance] = event.data.split(";");
                callback({ latitude: lat, longitude: long, distance })
            }

            if (this.#socket.OPEN) {
                this.#socket.addEventListener("message", messageHandler);
            }
        }
    }

    unsubscribe() {
        this.#socket.close();
    }
}