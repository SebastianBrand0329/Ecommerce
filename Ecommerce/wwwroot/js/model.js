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
            "url": "/Admin/Models/GetAll"
        },
        "columns": [
            { "data": "name", "width": "20%" },
            {
                "data": "state",
                "render": function (data) {
                    if (data == true) {
                        return "Activo";
                    } else {
                        return "Inactivo";
                    }
                }, "width": "20%"
            },
            {
                "data": "id",
                "render": function (data) {
                    return `<div class="text-center">
                    <a href="/Admin/Models/Upsert/${data}" class="btn btn-success text-white" style="cursor:pointer">
                    <i class="bi bi-pencil-square"></i> Edtar</a>

                    <a onclick=Delete("/Admin/Models/Delete/${data}") class="btn btn-danger text-white" style="cursor:pointer">
                                 <i class="bi bi-trash3-fill"></i> Eliminar
                                </a>
                    </div>`;
                }, "width": "20%"
            }
        ]
    });
}

function Delete(url) {
    swal({
        title: "¿Está seguro de Eliminar el modelo?",
        text: "Este registro no se podrá recuperar",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((borrar) => {
        if (borrar) {
            $.ajax({
                type: "POST",
                url: url,
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
    });
}