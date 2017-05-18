/*
 * auto-menubar.js
 * Script for handling lockscreen functionality
 */

//functions for handling login with password and override
//these should return true to allow and false to disallow

function handleLogin(password)
{
	doSend("Auth " + password);
}
var ovr = false;
function handleOverride()
{
	if(ovr == false)
	{
		alert("Are you sure you want to override this command?\n\nThis will immediatly trigger the alarm and send security emails to all programmed recipients.\n\nPress override again to continue.");
		ovr = true;
		return false;
	}
	else
	{
		doSend("alarm alarmnow Override")
		return true;
	}
}

//handler for showing/hiding the login lightbox
var lightboxHandler = function(ev)
{
    //abort if we've actually clicked on the lightbox
    if($(ev.target).is("#ls-lightbox-login *"))
    {
        return;
    }

    //clear the loginbox
    $('#ls-loginbox-input').val("");
    $('#ls-loginbox-message').text("");

    //toggle the lightbox
    $('#ls-lightbox-login').toggle();

    //focus the input box
    $('#ls-loginbox-input').focus();

};

//bind the handler to the whole screen
$(document).on("click", lightboxHandler);

//handlers for loginbox buttons
$('#ls-loginbox-clear').click(function(){
    $('#ls-loginbox-input').val("");
    $('#ls-loginbox-message').text("");
});

$('#ls-loginbox-cancel').click(function(){
    $('#ls-loginbox-input').val("");
    $('#ls-loginbox-message').text("");
    $("#ls-lightbox-login").hide();
});

$('#ls-loginbox-override').click(function(){
    if(handleOverride())
    {
        window.location.replace("index.html");
    }
});

$('#ls-loginbox-enter').click(function(){
    handleLogin($('#ls-loginbox-input').val())
});
var websocket;
var wsUri = "ws://127.0.0.1:5400/AtmComm";
function initWS()
{
websocket = new WebSocket(wsUri);
websocket.onopen = function(evt) { onOpen(evt) };
websocket.onclose = function(evt) { onClose(evt) };
websocket.onmessage = function(evt) { onMessage(evt) };
websocket.onerror = function(evt) { onError(evt) };
}
function onOpen(evt)
{
	console.log("WS Connection is now open");
}

function onClose(evt)
{
	console.log("WS Connection is now closed");
}

function onMessage(evt)
{
parseMessage(evt.data)
console.log('WS Server Response: ' + evt.data);
}

function onError(evt)
{
console.log('WS Server ERROR: ' + evt.data);
}
function doSend(message)
{
	if(websocket.readyState == 0)
	{
		console.log("WS is currently connecting, command ignored.");
		return;
	} else if(websocket.readyState == 1)
	{
		websocket.send(message);
		console.log("Command sent to WS server: " + message);
		return;
	} else if(websocket.readyState == 2)
	{
		console.log("WS is currently closing, command ignored");
		return;
	} else if(websocket.readyState == 3)
	{
		console.log("WS is currently closed, attempting to reconnect. Please send command again.");
		if(message == "keepalive")
		{
			document.getElementById('header-status').innerHTML = "Disconnected from server";
		}
		initWS();
		return;
	}
	console.log("state invalid");
}
function parseMessage(message)
{
	if(message == undefined)
		return;
	if(message.includes(":"))
	{
		var msg = message.split(":");
	}
	else
	{
		var msg = [message];
	}
	switch (msg[0])
	{
		case "Hello":
			console.log("hello received");
			break;
		case "Volume":
			document.getElementById('music-currentvol').innerHTML = "Volume: " + msg[1];
			break;
		case "Song":
			document.getElementById('ls-song').innerHTML = "Current Song: " + msg[1];
			break;
		case "Auth":
			if(msg[1] == "True")
			{
				window.location.replace("index.html?uid=" + $('#ls-loginbox-input').val());
			}
			else
			{
				$('#ls-loginbox-message').text("Sorry, that password is incorrect.");
				$('#ls-loginbox-input').val("");
			}
			break;
		case "Status":
			document.getElementById('ls-status').innerHTML = msg[1];
			break;
	}
}
function getInfo()
{	
	doSend("var song");
	doSend("var status");
    setTimeout(getInfo, 2000);
}
function updateClock() {
    var now = new Date();
    document.getElementById('ls-clock-datetime').innerHTML = now.toString('dddd, MMMM d yyyy, hh:mm:ss tt');
    setTimeout(updateClock, 100);
}
function keepAlive(){
	doSend("keepalive");
	console.log("sending keepalive");
    setTimeout(keepAlive, 5000);
}

