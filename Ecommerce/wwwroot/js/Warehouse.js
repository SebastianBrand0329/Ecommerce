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
            "url": "/Admin/Warehouses/GetAll"
        },
        "columns": [
            { "data": "name" , "width": "20%"},
            { "data": "description", "width": "40%" },
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
                    <a href="/Admin/Warehouses/Upsert/${data}" class="btn btn-success text-white" style="curos:pointer">
                    <i class="bi bi-pencil-square"></i> Edtar</a>
                    <a onclick=Delete("/Admin/Warehouses/Delete/${data}") class="btn btn-danger text-white" style="curos:pointer">
                    <i class="bi bi-trash3-fill"></i> Eliminar</a>
                    </div>`;
                }, "width": "20%"
            }
        ]
    });
}