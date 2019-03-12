$(document).ready(function () {

    function svidja() {
        $("#btnSvidja").on('click', function () {

            var data = {
                blogID: $("#BlogID").val()
            };
            console.log(data);

            $.get('/Blog/Svidja', data, function (result, status) {

                console.log('dodje');
                if (result.Success) {
                    if (result.Data) {
                        $("#svidjanje").html('<button class="btn btn-primary float-right" id="btnSvidja"><i id="ikonicaSvidja" class="fa fa-heart"></i>             Ne svidja mi se</button>');
                    } else {
                        $("#svidjanje").html('<button class="btn btn-secondary float-right" id="btnSvidja"><i class="fa fa-heart-o"></i>             Svidja mi se</button>');
                    }
                    svidja();
                }

            });

        });
    };

    $("#btnDodajSliku").on('click', function () {

        $("#modalDodajSliku").modal();

    });

    $("#btnSacuvaj").on('click', function () {

        var data = {
            slikaLink: $("#txtLinkSlike").val(),
            blogID: $("#BlogID").val()
        };

        $.get('/Blog/SlikaDodaj', data, function (result, status) {

            if (result.Success) {
                $("#slike").append(`<img class="col-3 pl-0" height="200" src="${data.slikaLink}" />`);
            }
            $("#txtLinkSlike").val('');
            $("#modalDodajSliku").modal('hide');

        });

    });

    $("#btnPosalji").on('click', function () {

        var data = {
            blogID: $("#BlogID").val(),
            komentar: $("#txtKomentar").val()
        };

        $.get('/Blog/PosaljiKomentar', data, function (result, status) {

            if (result.Success) {
                $("#komentari").append(`<div style="border:1px solid black" class="col-12 m-1">

                    <p>`+ result.Autor + ` (` + result.Datum + ')'+`</p>
                        <p>` + data.komentar + `</p>

            </div >`);
            }

            $("#txtKomentar").val('');

        });

    });

    svidja();

});