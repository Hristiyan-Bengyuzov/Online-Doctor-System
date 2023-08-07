var nav = new DayPilot.Navigator("nav");
nav.showMonths = 1;
nav.skipMonths = 1;
nav.selectMode = "month";
nav.onTimeRangeSelected = function (args) {
    dp.startDate = args.day;
    dp.update();
    dp.events.load("/api/events");
};

nav.init();

var dp = new DayPilot.Month("dp");
var antiForgeryToken = $('#antiForgeryForm input[name=__RequestVerificationToken]').val();
dp.onEventMove = function (args) {
    var params = {
        id: args.e.id(),
        start: args.newStart.toString(),
        end: args.newEnd.toString()
    };
    DayPilot.Http.ajax({
        method: 'PUT',
        url: '/api/events/' + args.e.id() + "/move",
        data: params,
        headers: {
            'X-CSRF-TOKEN': antiForgeryToken
        },
        success: function (ajax) {
            dp.message("Събитието е преместено");
        }
    });
};
dp.onEventResize = function (args) {
    var params = {
        id: args.e.id(),
        start: args.newStart.toString(),
        end: args.newEnd.toString()
    };
    DayPilot.Http.ajax({
        method: 'PUT',
        url: '/api/events/' + args.e.id() + "/move",
        data: params,
        headers: {
            'X-CSRF-TOKEN': antiForgeryToken
        },
        success: function (ajax) {
            dp.message("Събитието е преоразмерено");
        }
    });
};
dp.onBeforeEventRender = function (args) {
    args.data.backColor = args.data.color;
    args.data.areas = [
        { top: 3, right: 3, bottom: 3, icon: "icon-triangle-down", visibility: "Hover", action: "ContextMenu", style: "font-size: 12px; background-color: rgba(255, 255, 255, .5); border: 1px solid #aaa; padding: 3px; cursor:pointer;" }
    ];
};
dp.contextMenu = new DayPilot.Menu({
    items: [
        {
            text: "Delete",
            onClick: function (args) {
                var e = args.source;
                DayPilot.Http.ajax({
                    method: 'DELETE',
                    url: '/api/events/' + e.id(),
                    headers: {
                        'X-CSRF-TOKEN': antiForgeryToken
                    },
                    success: function (ajax) {
                        dp.events.remove(e);
                        dp.message("Събитието е изтрито");
                    }
                });
            }
        },
        {
            text: "-"
        },
        {
            text: "Син",
            icon: "icon icon-blue",
            color: "#a2c4c9",
            onClick: function (args) { updateColor(args.source, args.item.color); }
        },
        {
            text: "Зелен",
            icon: "icon icon-green",
            color: "#b6d7a8",
            onClick: function (args) { updateColor(args.source, args.item.color); }
        },
        {
            text: "Жълт",
            icon: "icon icon-yellow",
            color: "#ffe599",
            onClick: function (args) { updateColor(args.source, args.item.color); }
        },
        {
            text: "Червен",
            icon: "icon icon-red",
            color: "#ea9999",
            onClick: function (args) { updateColor(args.source, args.item.color); }
        },
        {
            text: "Без цвят",
            color: "auto",
            onClick: function (args) { updateColor(args.source, args.item.color); }
        },
    ]
});

dp.init();

dp.events.load("/api/events");


function updateColor(e, color) {
    var params = {
        color: color
    };
    DayPilot.Http.ajax({
        method: 'PUT',
        url: '/api/events/' + e.id() + '/color',
        data: params,
        headers: {
            'X-CSRF-TOKEN': antiForgeryToken
        },
        success: function (ajax) {
            e.data.color = color;
            dp.events.update(e);
            dp.message("Цветът беше променен");
        }
    });
}

var elements = {
    previous: document.querySelector("#previous"),
    today: document.querySelector("#today"),
    next: document.querySelector("#next"),
};

elements.previous.addEventListener("click", function (ev) {
    nav.select(nav.selectionDay.addMonths(-1));
});

elements.today.addEventListener("click", function (ev) {
    nav.select(DayPilot.Date.today());
});

elements.next.addEventListener("click", function (ev) {
    nav.select(nav.selectionDay.addMonths(1));
});
