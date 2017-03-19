/*
 * auto-common.js
 * Script file for scripts common to all inner panels
 */
 /*
var IDLE_TIMEOUT = 6; //seconds
var _idleSecondsCounter = 0;
document.onclick = function() {
  _idleSecondsCounter = 0;
};
document.onmousemove = function() {
  _idleSecondsCounter = 0;
};
document.onkeypress = function() {
  _idleSecondsCounter = 0;
};

var myInterval = window.setInterval(CheckIdleTime, 1000);

function CheckIdleTime() {
  _idleSecondsCounter++;
  console.log(IDLE_TIMEOUT - _idleSecondsCounter) + "";
  if (_idleSecondsCounter >= IDLE_TIMEOUT) {
    alert("Time expired!");
    window.clearInterval(myInterval);
  }
}*/
var sysstatus = "UNDEFINED";
var song = "UNDEFINED";
var musicstatus = "UNDEFINED";
var volume = "UNDEFINED";
var daymode = "UNDEFINED";
var hvacstatus = "UNDEFINED";
var alarmstatus = "UNDEFINED";
var powerstatus = "UNDEFINED";
var waterstatus = "UNDEFINED";
var kwdraw = "UNDEFINED";
var uid = getUrlVars()["uid"];
var nextevt = "UNDEFINED";
var wsUri = "ws://172.17.2.92:5400/AtmComm";
var websocket;
if(uid == null || uid == "")
{
    window.location.replace("lockscreen.html");
}
function getUrlVars()
{
	var vars = {};
	var parts = window.location.href.replace(/[?&]+([^=&]+)=([^&]*)/gi, function(m,key,value)
	{
		vars[key] = value;
	});
	return vars;
}
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
	}
	else if(websocket.readyState == 1)
	{
		websocket.send(message);
		console.log("Command sent to WS server: " + message);
		return;
	}
	else if(websocket.readyState == 2)
	{
		console.log("WS is currently closing, command ignored");
		return;
	}
	else if(websocket.readyState == 3)
	{
		console.log("WS is currently closed, attempting to reconnect. Please send command again.");
		if(message == "keepalive")
		{
			sysstatus = "Disconnected from server";
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
			volume = msg[1];
			break;
		case "Song":
			song = msg[1];
			break;
		case "Status":
			sysstatus = msg[1];
			break;
		case "AlarmStatus":
			alarmstatus = msg[1];
			break;
	}
}

function getInfo()
{
	doSend("var volume");
	doSend("var song");
	doSend("var status");
	doSend("var almstatus");
	setTimeout(getInfo, 2000);
}
function getMediaInfo()
{
	
	
}
function updateClock()
{
    var now = new Date();
    document.getElementById('header-clock').innerHTML = now.toString("dddd, MMMM d yyyy, hh:mm:ss tt");
    setTimeout(updateClock, 100);
}
function keepAlive()
{
	doSend("keepalive");
	console.log("sending keepalive");
    setTimeout(keepAlive, 5000);
}
$("#media-basiccontrol-play").click(function()
{
  doSend("$" + uid + " music play");
  getMediaInfo();
});
$("#media-basiccontrol-pause").click(function()
{
  doSend("$" + uid + " music pause");
  getMediaInfo();
});
$("#media-basiccontrol-next").click(function()
{
  doSend("$" + uid + " music next");
  getMediaInfo();
});
$("#media-basiccontrol-rescan").click(function()
{
  doSend("$" + uid + " music rescan");
  getMediaInfo();
});
$("#media-basiccontrol-volup").click(function()
{
  doSend("$" + uid + " music volup");
  getMediaInfo();
});
$("#media-basiccontrol-voldown").click(function()
{
  doSend("$" + uid + " music voldown");
  getMediaInfo();
});
$("#media-basiccontrol-resetrandom").click(function()
{
  doSend("$" + uid + " music resetrand");
  getMediaInfo();
});
$("#lighting-quick-allon").click(function()
{
  doSend("$" + uid + " lighting allon");
});
$("#lighting-quick-alloff").click(function()
{
  doSend("$" + uid + " lighting alloff");
});
$("#media-basiccontrol-mute").click(function()
{
  doSend("$" + uid + " music volume 0");
  getMediaInfo();
});
$("#sec-alarm-panic").click(function()
{
  doSend("alarm panic");
});
$("#sec-alarm-disarm").click(function()
{
  doSend("$" + uid + " alarm disarm panel");
});