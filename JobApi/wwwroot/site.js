const uri = "api/Job";
let jobs = null;
//hämtar information via interna api om antal objekt som finns med i databasen
function getCount(data) {
    const el = $("#counter");
    let name = "lediga tjänster";
    //kontrollerar antalet annonser och skriver ut rätt ord / även om det inte finns några annonser
    if (data) {
        if (data <= 1) {
            name = "aktiv tjänst";
        }
        el.text("Just nu finns det " + data + " " + name + " i Hudiksvalls kommun.");
    } else {
        el.text("Inga " + name);
    }
}
//När sidan är färdigladdad så kör den funktionen getData()
$(document).ready(function () {

    $(".searchBtn").on("click", function () {
        getData();
        $(".start").css({ display: "none" });
        $(".startList").css({ display: "block" });
    });

    $(".closeBtn").on("click", function () {
        closeView();
    });

    $("#overlay").on("click", function () {
        closeView();
    });
});
//hämtar information via interna apiet och skriver ut det i en lista//
function getData() {
    $.ajax({
        type: "GET",
        url: uri,
        cache: false,
        success: function (data) {
            const tBody = $("#list");

            $(tBody).empty();

            getCount(data.length);
            //loopar igenom varje object
            $.each(data, function (key, item) {
                const tr = $("<tr></tr>")
                    //lägga till dom hämtade fält från classen JobItem i c#
                    .append($("<td></td>").text(item.title))
                    .append($("<td></td>").text(item.dateEnd))
                    .append($("<td></td>").text(item.place))
                    .append($("<td></td>").text(item.region));

                tr.appendTo(tBody).on("click", function () {
                    viewItem(item.id);
                });
            });
            jobs = data;
        }
    });
}
//visar ytterligare information om annonsen
function viewItem(id) {
    $.each(jobs, function (key, item) {
        if (item.id === id) {
            $("#view-title").text(item.title);
            $("#view-place").text(item.place);
            $("#view-region").text(item.region);
            $("#view-text").text(item.text);
            $("#view-dateEnd").text(item.dateEnd);
        }
    });

    $("#overlay").css({ display: "block" });
    $("#view").css({ display: "block" });
    $("#view").css({ display: "block" });
    $("body").css({ overflow: "hidden" });
}

function closeView() {
    $("#overlay").css({ display: "none" });
    $("#view").css({ display: "none" });
    $("body").css({ overflow: "auto" });
}