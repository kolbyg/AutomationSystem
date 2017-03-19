/*
 * auto-menubar.js
 * Script for handling panel selection and titlebar buttons.
 */

//this is the actual panel selector
function selectPanel(panel)
{
    // console.log("selected panel " + panel);    
    var url = panel + ".html?uid=" + uid;    
    $('#iframe-panel-container').attr('src', url);
}

//menubar visual helper functions
function selectMenubarItem(item)
{
    $(item).addClass("menubar-item-selected");
}

function unselectMenubarItems()
{
    $("#menubar-list").children().removeClass("menubar-item-selected");
}

//click handler for menubar items
$("#menubar-list").children("li").click(function(){
    //display the correct menubar item
    unselectMenubarItems();
    selectMenubarItem(this);

    //get the proper name and call selectPanel
    var idparts = $(this).attr("id").split("-");
    var panel = idparts[idparts.length-1];
    selectPanel(panel);               
});

//click handler for console button
$("#header-console").click(function(){
    //default behaviour; feel free to change
    unselectMenubarItems();
    selectPanel("console");
});

//click handler for settings button
$("#header-settings").click(function(){
    //default behaviour; feel free to change
    unselectMenubarItems();
    selectPanel("settings");
});

//click handler for console button
$("#header-lock").click(function(){
    //default behaviour; feel free to change
    window.location.replace("lockscreen.html");
});


