const { ref, reactive, onMounted, createApp } = Vue;
createApp({
    setup() {
        const objectData = reactive({
            token: "",
            datePeriod: {
                start: "",
                end: ""
            },
            responseData: {
                list: []
            },
            dto: {}
        });

        const fetchData = (url) => {
            const url = `/admin/order/${url}`;
            axios.get(url).then((s) => {
                console.log(s.data);
                objectData.responseData.list = s.data.data;
                $('#dataTable').DataTable().destroy();
            }).catch((err) => { console.log(err) }).finally(() => { DtInit(); });
        }

        const Delete = (str) => {
            const url = `/Admin/service/Delete/${str}`
            //API
            Swal.fire({
                title: 'Are you sure?',
                text: "You won't be able to revert this!",
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                //cancelButtonColor: '#d33',
                confirmButtonText: 'Delete!'
            }).then((result) => {
                if (result.isConfirmed) {
                    console.log(url);
                    axios.delete(url).then((s) => {
                        console.log(s.data);
                        if (s.data.success) {
                            toastr.success(s.data.message);
                        } else {

                        }
                        fetchData();
                    }).catch((err) => { console.log(err) });
                }
            });
        }

        onMounted(() => {
            const url = window.location.search;
            if (url.includes("approved")) {
                fetchData("GetAllApprovedOrders");
            } else {
                if (url.includes("all")) {
                    fetchData("GetAllOrders");
                } else {
                    fetchData("GetAllPendingOrders");
                }
            }
        });

        return {
            objectData,
            Delete
        }
    }
}).mount('#admin_Order_Index');

// #region [表格初始化]
function DtInit() {
    $('#dataTable').DataTable({
        language: { url: `../lib/datatable/CHT.json` },
        //lengthMenu: [[15, 50, -1], [15, 50, "全部"]],
        lengthMenu: [[-1], ["全部"]],
        columnDefs: [
            { "width": "20%", "targets": 0 },
            { "width": "20%", "targets": 1 },
            { "width": "15%", "targets": 2 },
            { "width": "15%", "targets": 3 },
            { "width": "15%", "targets": 4 },
            { "width": "15%", "targets": 5 }
        ],
        dom: "ftip",
        //ordering: false,
        deferRender: true,
        autoWidth: false,
        destroy: true,
        retrieve: true,
        initComplete: function () { },
        footerCallback: function () { }
    });
}
// #endregion