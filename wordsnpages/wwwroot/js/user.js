var dataTable;
$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": '/user/getall'
        },
        "columns": [
            { "data": 'name', "width": "15%" },
            { "data": 'email', "width": "15%" },
            { "data": 'phoneNumber', "width": "15%" },
            { "data": 'company.name', "width": "15%" },
            { "data": 'role', "width": "15%" },
            {
                "data": {id: 'id', lockoutEnd: 'lockoutEnd'},
                "render": function (data) {
                    var today = new Date().getTime();
                    var lockout = new Date(data.lockoutEnd).getTime(); //converting lockoutEnd to time

                    if (lockout > today) {
                        //user is lockedout and we need to display button to unlock it
                        return '<div class="text-center">' +
                            `<a onclick="LockUnlock('${data.id}')" class="btn btn-danger text-white" style="cursor:pointer; width:150px;">` +
                            '<i class="bi bi-unlock-fill"> Lock </i>' +
                            '</a>' +
                  
                            `<a href="/user/RoleManagement?userId=${data.id}" class="btn btn-success text-white" style="cursor:pointer; width:150px;">` +
                            '<i class="bi bi-pencil-square"> Permission </i>' +
                            '</a>' +

                            '</div>';                    }
                    else {
                        return '<div class="text-center">' +
                            `<a onclick="LockUnlock('${data.id}')" class="btn btn-success text-white" style="cursor:pointer; width:150px;">` +
                            '<i class="bi bi-unlock-fill"> Unlock </i>' +
                            '</a>' +
                            `<a href="/user/RoleManagement?userId=${data.id}" class="btn btn-success text-white" style="cursor:pointer; width:150px;">` +
                            '<i class="bi bi-pencil-square"> Permission </i>' +
                            '</a>' +
                            '</div>';
                    }


                },
                "width": "25%"
            }
        ]

    });
}

function Delete(url) {
    Swal.fire({
        title: "Are you sure?",
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, delete it!"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) {
                    dataTable.ajax.reload();
                    toastr.success(data.message);

                }
            });
        }
    });
}

function LockUnlock(id) {
    $.ajax({
        type: "POST",
        url: '/User/LockUnlock',
        data: JSON.stringify(id),
        contentType: "application/json",
        success: function (data) {
            if (data.success) {
                toastr.success(data.message);
                dataTable.ajax.reload();
            }
        }
    });
}