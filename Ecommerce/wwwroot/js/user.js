let databable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    databable = $('#tblData').DataTable({
        "language": {
            "lengthMenu": "Mostrar _MENU_ Registros Por Pagina",
            "zeroRecords": "Ningun Registro",
            "info": "Mostrar page _PAGE_ de _PAGES_",
            "infoEmpty": "no hay registros",
            "infoFiltered": "(filtered from _MAX_ total registros)",
            "search": "Buscar",
            "paginate": {
                "first": "Primero",
                "last": "Último",
                "next": "Siguiente",
                "previous": "Anterior"
            }
        },
        "ajax": {
            "url": "/Admin/Users/GetAll"
        },
        "columns": [
            { "data": "email" },
            { "data": "name" },
            { "data": "lastName" },
            { "data": "phoneNumber" },
            { "data": "role" },
            {
                "data": { id: "id", lockoutEnd: "lockoutEnd" },
                "render": function (data) {
                    let today = new Date().getTime();
                    let block = new Date(data.lockoutEnd).getTime();
                    if (block > today) {
                        // User Block
                        return `<div class="text-center">
                    <a onclick=unlock('${data.id}') class="btn btn-danger text-white" style="cursor:pointer", width:150px>
                                 <i class="bi bi-unlock-fill"></i> Desbloquear
                                </a>
                    </div>`;
                    } else {
                        return `<div class="text-center">
                    <a onclick=unlock('${data.id}') class="btn btn-success text-white" style="cursor:pointer, width:150px">
                                 <i class="bi bi-lock-fill"></i> Bloquear
                                </a>
                    </div>`;
                    }

                }
            }
        ]
    });
}

function unlock(id)
{

    $.ajax({
        type: "POST",
        url: '/Admin/Users/unlock',
        data: JSON.stringify(id),
        contentType: "application/json",
        success: function (data) {
            if (data.success) {
                toastr.success(data.message);
                databable.ajax.reload();
            } else {
                toastr.error(data.message);
            }
        }
    });
}