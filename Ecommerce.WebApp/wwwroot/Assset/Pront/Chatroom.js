mychatroom = new Vue({
    el: "#chatroom",
    components: {
        message: {
            props: ['sender', 'message', 'createdat'],
            template: '<div><b>{{ sender }}</b> <sub class="createdat pt-2">{{ createdat | showChatTime }}</sub><p>{{ message }}</p></div>',
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
        productid: '',
        userids: '',
        currentUserName: null,
        arrayUsers: []
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
            this.connection.on("ReceiveMessage", function (group, message) {
                mychatroom.messages.push(message);

                for (var i = 0; i < mychatroom.chatroomlist.messages.length; i++) {
                    mychatroom.messages[i].status = false;
                }
            });
            this.connection.on("AllReceiveMessage", function (group, message) {
                for (var i = 0; i < mychatroom.chatroomlist.chats.length; i++)
                {
                    if (mychatroom.chatroomlist.chats[i].userIDs == group) {
                        mychatroom.chatroomlist.chats[i].unReaded += 1;
                    }
                   
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
    update() {
        this.adjustChatContainer();
    },
    methods: {
       
        adjustChatContainer: function () {
            var chatContainer = document.getElementById('messages');
            if (chatContainer) {
                chatContainer.scrollTop = chatContainer.scrollHeight;
            }
            this.chatroomlist.message.mes = '';
        },
        viewmessage: function (userids) {
            var self = this;
            
            if (self.productid != 0) {
                this.connection.invoke("LeaveGroup", this.PrivateName).catch(function (err) {
                    return console.error(err.toString());
                });
            }
           
            this.PrivateName = userids;
            fetch(baseUrl + "chatlist/" + this.PrivateName+'/'+0)
                .then(function (response) {
                    return response.json();
                })
                .then(function (result) {
                    
                    
                    for (var i = 0; i < result.messages.length; i++) {
                        result.messages[i].status = false;
                    }
                    self.bind(result);
                    self.loading = false;
                    self.isLoaded = true;
                    

                })
                .catch(function (error) {
                    console.log("error:", error);
                });

            this.connection.invoke("JoinGroup", this.PrivateName).catch(function (err) {
                return console.error(err.toString());
            });
        },
       
        sendMessage: function (event) {

            var self = this;
            if (self.chatroomlist.message.mes.trim() == '' || self.chatroomlist.message.mes.trim == null) {
                return;
            }
            //this.connection.invoke("SendMessageToGroup", self.PrivateName, self.chatroomlist.message.mes).catch(function (err) {
            //    return console.error(err.toString());
            //});
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
            for (var i = 0; i < this.chatroomlist.chats.length; i++) {
                if (this.chatroomlist.chats[i].userIDs == self.PrivateName) {
                    this.chatroomlist.chats[i].unReaded = 0;
                }

            }
           
            this.adjustChatContainer();
        },
        
        load: function (userids,productid) {
            var self = this;
            self.userids = userids;
            self.productid = productid;
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
        deletechatroom: function (chatid) {
            self = this;
            fetch(baseUrl + "deletechatroom/" + chatid)
                .then(function (response) {
                    return response.json();
                })
                .then(function (result) {
                        notifications.push(result);
                })
                .catch(function (error) { console.log("error:", error); });
        },
        deletechat: function (chatid) {
            var self = this;

            var chatInfo = "";
            if (chatid) {
                chatInfo += ' <br/>"' + chatid + '"';
            }
            else {
                return;
            }

            alert.open({
                title: "Delete chat",
                body: "delete chat confirm" + chatInfo,
                confirmCss: "btn-danger",
                confirmIcon: "fa fa-trash",
                confirmText: "delete",
                onConfirm: function () {
                    fetch(baseUrl + "deletechat/" + chatid)
                        .then(function (response) {
                            ok = response.ok;
                            return response.json();
                        })
                        .then(function (result) {
                            if (result.senderStatus == 0 && result.receiverStatus == 0) {
                                self.deletechatroom(chatid);
                            }
                            self.load(self.userids, self.productid);
                        })
                        .catch(function (error) {
                            console.log("error:", error);

                            notifications.push({
                                body: error,
                                type: "danger",
                                hide: true
                            });
                        });
                }
            });
        }
    }

});