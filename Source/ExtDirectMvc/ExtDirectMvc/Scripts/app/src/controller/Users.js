Ext.define('ExNet.controller.Users', {
    extend: 'Ext.app.Controller',

    views: ['UserList'],
    stores: ['Users'],

    init: function () {
        this.control({
            'userlist': {
                itemdblclick: this.editUser
            }
        });
    },

    editUser: function (grid, record) {
        Ext.log('Double clicked on ', record.get('name'));
        //ExNet.TestAction.doEcho(1,2);

        Ext.Ajax.request({
            url: 'Home/TestAction',
            jsonData: { nombre: 'erick', apellido: 'lombardo' }
        });
    }
});