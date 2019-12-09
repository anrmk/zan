class AdminController {
    constructor(options, callback = (target) => { }) {

        this.hubConnection = null; //соединение с хабом signalR
        this._hubInit();
        this.callback = callback;

        this.callback(this);
    }
    _hubInit() {
        this.hubConnection = new signalR.HubConnectionBuilder().withUrl("/syncDataHub").build();
        this.hubConnection.on("syncNsiResult", (data) => {
            this._syncNsiResult(data);
        });
        this.hubConnection.onclose(async () => { setTimeout(() => this._hubStartConnection(), 10000); });
        this._hubStartConnection();
    }

    async _hubStartConnection() {
        await this.hubConnection.start().then(() => {
            console.log("syncDataHub connected");
            //connection.invoke('getConnectionId')
            //    .then(function (connectionId) {
            //        sessionStorage.setItem('conectionId', connectionId);
            //        // Send the connectionId to controller
            //    }).catch(err => console.error(err.toString()));;
        }).catch(err => {
            setTimeout(() => this._hubStartConnection(), 5000);
            return console.log(err);
        });
    }
    _syncNsiResult(data) {
        if (data != null) {
            $(`#${data.name} .description`).text(`Количество записей: ${data.count}. Время обработки: ${data.leadTime}`)
        }
    }
}