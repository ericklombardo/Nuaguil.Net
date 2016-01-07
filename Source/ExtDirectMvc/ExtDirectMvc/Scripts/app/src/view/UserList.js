Ext.define('ExNet.view.UserList', {
    extend: 'Ext.grid.Panel',
    alias: 'widget.userlist',

    title: 'My User List',
    store: 'Users',
    
    initComponent: function () {
        this.columns = [
            { header: 'Name', dataIndex: 'name', flex: 1 },
            { header: 'Email', dataIndex: 'email', flex: 1 }
        ];

        this.callParent(arguments);
    }
});