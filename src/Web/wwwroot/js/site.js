// Отрисовка дерева
$('#jstree_div').jstree({
    'core': {
        'data': {
            'url': '/GetTree/',
            'data': function (node) {
                return { 'id': node.id };
            }
        }
    }
});

// Обработка нажатий на элементы дерева
$('#jstree_div').on('select_node.jstree', function (e, data) {
    $('#mainBlock').load(`/Details/` + data.node.id);
});

cancel_click = () => load_page("blankpage", '');
create_click = (id) => load_page("create", id);
update_click = (id) => load_page("update", id);
delete_click = (id) => load_page("delete", id);

function load_page(page, id) {
    $('#mainBlock').load(`${page}/${id}`);
}

async function refresh_tree(id) {
    var t = $("#jstree_div").jstree(true);
    $("#jstree_div").one("refresh.jstree", function () { t.select_node(id); })
    t.refresh();
}