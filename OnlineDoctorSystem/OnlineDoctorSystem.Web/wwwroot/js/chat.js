var connection =
    new signalR.HubConnectionBuilder()
        .withUrl("/chat")
        .withAutomaticReconnect()
        .build();

connection.on("NewMessage", function (message) {
    var chatInfo = "";

    if (message.isDoctor) {
        chatInfo += "<div class='media bg-success text-white rounded text-wrap' style='margin-bottom: 5px'>";
    }
    if (message.isAdmin) {
        chatInfo += "<div class='media bg-warning text-white rounded text-wrap' style='margin-bottom: 5px'>";
    }
    if (message.isPatient) {
        chatInfo += "<div class='media bg-gray-600 text-white rounded text-wrap' style='margin-bottom: 5px'>";
    }

    chatInfo += "<div class='media-body'>";
    chatInfo += "<div class='d-flex'>";
    chatInfo += `<img src='${message.imageUrl}' style='height: 90px; width: 90px' class='mr-3' alt='Снимка'>`;
    chatInfo += "<div style='padding-left: 15px;'>"; 
    chatInfo += `<h3 class='mt-0'>${escapeHtml(message.text)}</h3>`;
    chatInfo += `<h5 class='mt-0'>${message.user}</h5>`;
    chatInfo += `<h5 class='mt-0'>Изпратено на: ${message.createdOn}</h5>`;
    chatInfo += "</div></div></div></div>";

    $("#messagesList").append(chatInfo);
});
$("#sendButton").click(function () {
    var message = $("#messageInput").val();
    if (message !== "") {
        connection.invoke("Send", message);
        $("#messageInput").val("");
    }
});

connection.start().catch(function (err) {
    return console.error(err.toString());
});

function escapeHtml(unsafe) {
    return unsafe
        .replace(/&/g, "&amp;")
        .replace(/</g, "&lt;")
        .replace(/>/g, "&gt;")
        .replace(/"/g, "&quot;")
        .replace(/'/g, "&#039;");
}