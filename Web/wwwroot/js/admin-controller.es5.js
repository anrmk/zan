"use strict";

var _createClass = (function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; })();

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

var AdminController = (function () {
    function AdminController(options) {
        var callback = arguments.length <= 1 || arguments[1] === undefined ? function (target) {} : arguments[1];

        _classCallCheck(this, AdminController);

        this.hubConnection = null; //соединение с хабом signalR
        this._hubInit();
        this.callback = callback;

        this.callback(this);
    }

    _createClass(AdminController, [{
        key: "_hubInit",
        value: function _hubInit() {
            var _this = this;

            this.hubConnection = new signalR.HubConnectionBuilder().withUrl("/syncDataHub").build();
            this.hubConnection.on("syncNsiResult", function (data) {
                _this._syncNsiResult(data);
            });
            this.hubConnection.onclose(function callee$2$0() {
                return regeneratorRuntime.async(function callee$2$0$(context$3$0) {
                    var _this2 = this;

                    while (1) switch (context$3$0.prev = context$3$0.next) {
                        case 0:
                            setTimeout(function () {
                                return _this2._hubStartConnection();
                            }, 10000);
                        case 1:
                        case "end":
                            return context$3$0.stop();
                    }
                }, null, _this);
            });
            this._hubStartConnection();
        }
    }, {
        key: "_hubStartConnection",
        value: function _hubStartConnection() {
            return regeneratorRuntime.async(function _hubStartConnection$(context$2$0) {
                var _this3 = this;

                while (1) switch (context$2$0.prev = context$2$0.next) {
                    case 0:
                        context$2$0.next = 2;
                        return regeneratorRuntime.awrap(this.hubConnection.start().then(function () {
                            console.log("syncDataHub connected");
                            //connection.invoke('getConnectionId')
                            //    .then(function (connectionId) {
                            //        sessionStorage.setItem('conectionId', connectionId);
                            //        // Send the connectionId to controller
                            //    }).catch(err => console.error(err.toString()));;
                        })["catch"](function (err) {
                            setTimeout(function () {
                                return _this3._hubStartConnection();
                            }, 5000);
                            return console.log(err);
                        }));

                    case 2:
                    case "end":
                        return context$2$0.stop();
                }
            }, null, this);
        }
    }, {
        key: "_syncNsiResult",
        value: function _syncNsiResult(data) {
            if (data != null) {
                $("#" + data.name + " .description").text("Количество записей: " + data.count + ". Время обработки: " + data.leadTime);
            }
        }
    }]);

    return AdminController;
})();

