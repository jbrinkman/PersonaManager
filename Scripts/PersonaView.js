var PersonaManager = PersonaManager || {};

PersonaManager.personaListViewModel = function (moduleId, resx) {
    var service = {
        path: "PersonaManager",
        framework: $.ServicesFramework(moduleId)
    }
    service.baseUrl = service.framework.getServiceRoot(service.path) + "Persona/";
    service.baseSwitchUrl = service.framework.getServiceRoot(service.path) + "Switch/";

    var isLoading = ko.observable(false);
    var personaList = ko.observableArray([]);
    var editMode = ko.observable(false);
    var addUrl = ko.observable('');

    var init = function () {
        getpersonaList();
    }

    var getpersonaList = function () {
        isLoading(true);
        var jqXHR = $.ajax({
            url: service.baseUrl,
            beforeSend: service.framework.setModuleHeaders,
            dataType: "json"
        }).done(function (data) {
            if (data) {
                load(data);
            }
            else {
                // No data to load 
                personaList.removeAll();
            }
        }).always(function (data) {
            isLoading(false);
        });
    };

    var deletePersona = function (persona) {
        isLoading(true);
        var restUrl = service.baseUrl + persona.id();
        var jqXHR = $.ajax({
            method: "DELETE",
            url: restUrl,
            beforeSend: service.framework.setModuleHeaders
        }).done(function () {
            console.log("Deleted: " + persona.id());
            personaList.remove(persona);
        }).fail(function () {
            console.log("Error");
        }).always(function (data) {
            console.log("finished");
            isLoading(false);
        });
    }

    var load = function (data) {
        addUrl(data.addUrl);
        editMode(data.editMode);
        personaList.removeAll();
        if (data.personas) {
            var underlyingArray = personaList();
            for (var i = 0; i < data.personas.length; i++) {
                var result = data.personas[i];
                var item = new PersonaManager.personaViewModel();
                item.load(result);
                underlyingArray.push(item);
            }
            personaList.valueHasMutated();
        }
    };

    var showDrawer = function () {
        $('.persona-manager').addClass('show-drawer');
    };

    var hideDrawer = function () {
        $('.persona-manager').removeClass('show-drawer');
    };

    var switchUser = function (persona) {
        isLoading(true);
        var restUrl = service.baseSwitchUrl + persona.assignedUser();
        var jqXHR = $.ajax({
            url: restUrl,
            beforeSend: service.framework.setModuleHeaders,
            dataType: "json"
        }).done(function (data) {
            location.reload();
        }).always(function (data) {
            isLoading(false);
        });
    };

    return {
        init: init,
        load: load,
        personaList: personaList,
        addUrl: addUrl,
        getpersonaList: getpersonaList,
        deletePersona: deletePersona,
        editMode: editMode,
        isLoading: isLoading,
        show: showDrawer,
        hide: hideDrawer,
        switchUser: switchUser
    }
};

PersonaManager.personaViewModel = function () {
    var self = this;
    self.id = ko.observable('');
    self.name = ko.observable('');
    self.description = ko.observable('');
    self.assignedUser = ko.observable(-1);

    self.userPic = function () {
        return 'DnnImageHandler.ashx?mode=profilepic&userId=' + self.assignedUser() + '&h=100&w=100';
    };
    self.editUrl = ko.observable('');

    self.load = function (data) {
        self.id(data.id)
        self.name(data.name);
        self.assignedUser(data.assignedUser);
        self.description(data.description);
        self.editUrl(data.editUrl);
    };

    return {
        id: self.id,
        name: self.name,
        description: self.description,
        assignedUser: self.assignedUser,
        userPic: self.userPic,
        editUrl: self.editUrl,
        load: self.load
    }
}
