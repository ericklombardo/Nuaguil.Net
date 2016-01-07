Ext.Loader.setConfig({
    enabled: true
});
Ext.application({
    requires: ['Ext.direct.Manager', 'Ext.direct.RemotingProvider', 'Ext.container.Viewport'],
    name: 'ExNet',
    appFolder: 'Scripts/app/src',
    controllers: ['Users'],

    launch: function () {

        Ext.direct.Manager.addProvider({
            "type":"remoting",
            "url":"router", 
            "actions":{     
                "TestAction":[       
                {
                    "name":"doEcho",
                    "len":2
                },{
                    "name":"multiply",
                    "len":1
                },{
                    "name":"doForm",
                    "formHandler":true, // handle form on server with Ext.Direct.Transaction
                    "len":1
                }]
            },
            "namespace":"ExNet"
        });

        Ext.create('Ext.container.Viewport', {
            layout: 'fit',
            items: [
                {
                    xtype: 'userlist'
                }
            ]
        });
    }
});