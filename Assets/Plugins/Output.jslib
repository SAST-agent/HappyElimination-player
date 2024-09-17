mergeInto(LibraryManager.library, {
  Send_frontend: function (str) {
    window.handlePlayerCalls(UTF8ToString(str));
  },
  Connect_ws: function(address) {
    window.Connect_ws(UTF8ToString(address));
  },
  Send_ws: function(str_payload) {
    window.Send_ws(UTF8ToString(str_payload));
  },
  Getoperation: function(index) {
     window.SendOperation( index );
  },
});