$(document).ready(function () {

    function pretraziBlogove() {
        var data = {
            tekstPretraga: $("#txtPretraga").val(),
            drzavaID: $("#Drzave").val()
        };

        $.get("/Pocetna/Pretraga", data, function (result, status) {

            $("#blogovi").html(result);

        });

    };

    $("#btnPretrazi").on('click', function () {

        pretraziBlogove();

    });

    $("#btnNoviBlog").on('click', function () {
        window.location.href = "/Blog/Create";
    });

    pretraziBlogove();

});