mychatroom = new Vue({
    el: "#chatroom",
    components: {
        message: {
            props: ['sender', 'message', 'createdat'],
            template: '<div><b>{{ sender }}</b> <sub class="createdat">{{ createdat | showChatTime }}</sub><p>{{ message }}</p></div>',
            filters: {
                showChatTime: function (createdat) {
                    var date = new Date(createdat);
                    date = ("0" + date.getDate()).slice(-2) + '/' + ("0" + date.getMonth()).slice(-2) + '/' + date.getFullYear() + ' ' +
                        ("0" + date.getHours()).slice(-2) + ':' + ("0" + date.getMinutes()).slice(-2);
                    return date;
                }
            }
        },
    },
    data: {
        loading: true,
        Messages: [],
        chatroomlist: Object,
        chatroomlisttemp: Object,
        connection: Object,
        PrivateName: '',
        messages: [],
        message: '',
        isTyping: '',
        isLoaded:false,
        onlineUsers: [],
        currentUserName: null
    },
    created: function () {
      
    },
    mounted: function () {
        this.connection = new signalR.HubConnectionBuilder()
            .withUrl("/chatHub").build();


        this.connection.on("UserConnected", function (connectionId) {
            //var groupElement = document.getElementById("group");
            //var option = document.createElement("option");
            //option.text = connectionId;
            //option.value = connectionId;

        });
        this.connection.on("UserDisconnected", function (connectionId) {
            //var groupElement = document.getElementById("group");
            //for (var i = 0; i < groupElement.length; i++) {
            //    if (groupElement.option[i].value == connectionId) {
            //        groupElement.remove(i);
            //    }
            //}
        });



        this.connection.start().then(function () {
            document.getElementById("sendButton").disabled = false;
        }).catch(function (err) {
            return console.error(err.toString());
        });

       

        window.addEventListener('load', () => {
            this.connection.invoke("JoinGroup", this.PrivateName).catch(function (err) {
                return console.error(err.toString());
            });
            if (this.isLoaded) {
                this.PrivateName = this.chatroomlist.chatroom.userIDs;
                this.chatroomlist = this.chatroomlist;
                this.isLoaded = false;
            }
            this.connection.on("ReceiveMessage", function (message) {
                mychatroom.messages = [];
                for (var i = 0; i < mychatroom.chatroomlist.messages.length; i++) {
                    mychatroom.messages.push(mychatroom.chatroomlist.messages[i]);
                }
            });
            //this.connection.on("ReceiveMessage", function (message) {
            //    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
            //    var encodedMsg = " says " + msg;
            //    var div = document.createElement("div");
            //    div.innerHTML = msg + "<hr/>";
            //    var mes = document.getElementById("messagesID").appendChild(div);
            //});
        })
    },
    updated: function () {
        var messages = this.chatroomlist.messages;
        
        this.adjustChatContainer();
    },
    methods: {
        adjustChatContainer: function () {
            var chatContainer = document.getElementById('messages');
            if (chatContainer) {
                chatContainer.scrollTop = chatContainer.scrollHeight;
            }
        },
        viewmessage: function (userids) {
            var self = this;
            this.connection.invoke("LeaveGroup", this.PrivateName).catch(function (err) {
                return console.error(err.toString());
            });
            this.PrivateName = userids;
            fetch(baseUrl + "chatlist/" + this.PrivateName+'/'+0)
                .then(function (response) {
                    return response.json();
                })
                .then(function (result) {
                    self.bind(result);
                    self.loading = false;
                    self.isLoaded = true;
                    

                })
                .catch(function (error) {
                    console.log("error:", error);
                });

            this.connection.invoke("JoinGroup", userids).catch(function (err) {
                return console.error(err.toString());
            });
        },
        SendButton: function () {
            var message = document.getElementById("message").value;
            this.connection.invoke("SendMessageToGroup", this.PrivateName, message).catch(function (err) {
                return console.error(err.toString());
            });
            
        },
        sendMessage: function (event) {
            var self = this;
            if (self.chatroomlist.message.mes.trim() == '' || self.chatroomlist.message.mes.trim == null) {
                return;
            }
            this.connection.invoke("SendMessageToGroup", self.PrivateName, self.chatroomlist.message.mes).catch(function (err) {
                return console.error(err.toString());
            });
            self.adjustChatContainer();
            //self.messages.push(self.message);
            fetch(baseUrl + "SaveMessages", {
                method: "post",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify(self.chatroomlist)
            })
                .then(function (response) {
                    return response.json();
                })
                .then(function (result) {
                    self.bind(result);
                   
                    self.loading = false;
                    
                    
                })
                .catch(function (error) { console.log("error:", error); });


            
        },
        bind: function (result) {
           
            this.chatroomlist = result;
            this.PrivateName = this.chatroomlist.chatroom.userIDs;
            this.message = '';
            this.messages = [];
            for (var i = 0; i < this.chatroomlist.messages.length; i++) {
                this.messages.push(this.chatroomlist.messages[i]);
            }
            this.adjustChatContainer();
        },
        load: function (userids,productid) {
            var self = this;
            //self.PrivateName = groupName;
            fetch(baseUrl + "chatlist/" + userids + '/' + productid)
                .then(function (response) {
                    return response.json();
                })
                .then(function (result) {

                    self.loading = false;
                    self.isLoaded = true;
                    self.bind(result);
                   
                })
                .catch(function (error) { console.log("error:", error); });
        },
    }

});