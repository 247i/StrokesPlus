window.gestures = new Gestures();

class SectionGlobalActions {
    constructor(application) {
        this.Application = application;
        this.ActionList = new ActionList('tabGlobalActionsActionList', this.Application, 50, 50);
        this.Load();
    }

    get CurrentApplication() {
        return this.Application;
    }

    get CurrentActionItem() {
        return this.ActionList.CurrentAction;
    }

    get HTML() {
        return `<div>
                    <!-- Tabs -->
                    <ul class="nav nav-tabs nav-justified">
                        <li class="nav-item" title="${stringEscapeProperty(`||tabGlobalActionsTabName||`)}">
                            <a class="nav-link active" data-toggle="tab" href="#tabGlobalActionsActionList">${stringEscapeHtml(`||tabGlobalActionsTabName||`)}</a>
                        </li>
                        <li class="nav-item" title="${stringEscapeProperty(`||TextExpansion||`)}">
                            <a class="nav-link" data-toggle="tab" href="#tabGlobalActionsTextExpansion">${stringEscapeHtml(`||TextExpansion||`)}</a>
                        </li>
                        <li class="nav-item" title="${stringEscapeProperty(`||tabGlobalSettingsTabName||`)}">
                            <a class="nav-link" data-toggle="tab" href="#tabGlobalActionsSettings">${stringEscapeHtml(`||tabGlobalSettingsTabName||`)}</a>
                        </li>
                        <li class="nav-item" title="${stringEscapeProperty(`||tabGlobalMouseEventsTabName||`)}">
                            <a class="nav-link" data-toggle="tab" href="#tabGlobalActionsMouseEvents">${stringEscapeHtml(`||tabGlobalMouseEventsTabName||`)}</a>
                        </li>
                        <li class="nav-item" title="${stringEscapeProperty(`||tabGlobalLoadUnloadTabName||`)}">
                            <a class="nav-link" data-toggle="tab" href="#tabGlobalActionsLoadUnload">${stringEscapeHtml(`||tabGlobalLoadUnloadTabName||`)}</a>
                        </li>
                        <li class="nav-item" title="${stringEscapeProperty(`||tabGlobalWindowEventsTabName||`)}">
                            <a class="nav-link" data-toggle="tab" href="#tabGlobalActionsWindowEvents">${stringEscapeHtml(`||tabGlobalWindowEventsTabName||`)}</a>
                        </li>
                    </ul>

                    <!-- Tab panes -->
                    <div class="tab-content border-top">
                        <div class="tab-pane active" id="tabGlobalActionsActionList">
                            ${this.ActionList.HTML}
                        </div>
                        <div class="tab-pane" id="tabGlobalActionsTextExpansion">
                            GLOBAL TEXT EXPANSIONS
                        </div>
                        <div class="tab-pane" id="tabGlobalActionsSettings">
                            GLOBAL ACTION SETTINGS
                        </div>
                        <div class="tab-pane" id="tabGlobalActionsMouseEvents">
                            GLOBAL ACTION MOUSE EVENTS
                            <button type="button" class="btn btn-primary"
                                data-toggle="modal"
                                data-target="#modalStandardCenter"
                                data-title="Test Title"
                                data-body="Test body"
                                data-buttons="yes|settingsApplyClickedCloseConfirm,no|modalDismissStandardCenter">
                                Yes No Modal
                            </button>
                            <button type="button" class="btn btn-primary"
                                data-toggle="modal"
                                data-target="#modalStandardCenter"
                                data-title="Apply Title"
                                data-body="Apply body"
                                data-buttons="apply,ok,close">
                                    Apply OK Close Modal
                            </button>
                            <p>GLOBAL ACTION MOUSE EVENTS</p>
                            <p>GLOBAL ACTION MOUSE EVENTS</p>
                            <p>GLOBAL ACTION MOUSE EVENTS</p>
                            <p>GLOBAL ACTION MOUSE EVENTS</p>
                            <p>GLOBAL ACTION MOUSE EVENTS</p>
                            <p>GLOBAL ACTION MOUSE EVENTS</p>
                            <p>GLOBAL ACTION MOUSE EVENTS</p>
                            <p>GLOBAL ACTION MOUSE EVENTS</p>
                            <p>GLOBAL ACTION MOUSE EVENTS</p>
                            <p>GLOBAL ACTION MOUSE EVENTS</p>
                            <p>GLOBAL ACTION MOUSE EVENTS</p>
                            <p>GLOBAL ACTION MOUSE EVENTS</p>
                            <p>GLOBAL ACTION MOUSE EVENTS</p>
                            <p>GLOBAL ACTION MOUSE EVENTS</p>
                            <p>GLOBAL ACTION MOUSE EVENTS</p>
                            <p>GLOBAL ACTION MOUSE EVENTS</p>
                            <p>GLOBAL ACTION MOUSE EVENTS</p>
                            <p>GLOBAL ACTION MOUSE EVENTS</p>
                            <p>GLOBAL ACTION MOUSE EVENTS</p>
                            <p>GLOBAL ACTION MOUSE EVENTS</p>
                            <p>GLOBAL ACTION MOUSE EVENTS</p>
                            <p>GLOBAL ACTION MOUSE EVENTS</p>
                            <p>GLOBAL ACTION MOUSE EVENTS</p>
                            <p>GLOBAL ACTION MOUSE EVENTS</p>
                            <p>GLOBAL ACTION MOUSE EVENTS</p>
                            <p>GLOBAL ACTION MOUSE EVENTS</p>
                            <p>GLOBAL ACTION MOUSE EVENTS</p>
                            <p>GLOBAL ACTION MOUSE EVENTS</p>
                            <p>GLOBAL ACTION MOUSE EVENTS</p>
                            <p>GLOBAL ACTION MOUSE EVENTS</p>
                            <p>GLOBAL ACTION MOUSE EVENTS</p>
                            <p>GLOBAL ACTION MOUSE EVENTS</p>
                            <p>GLOBAL ACTION MOUSE EVENTS</p>
                            <p>GLOBAL ACTION MOUSE EVENTS</p>
                            <p>GLOBAL ACTION MOUSE EVENTS</p>
                            <p>GLOBAL ACTION MOUSE EVENTS</p>
                            <p>GLOBAL ACTION MOUSE EVENTS</p>
                            <p>GLOBAL ACTION MOUSE EVENTS</p>
                            <p>GLOBAL ACTION MOUSE EVENTS</p>
                            <p>GLOBAL ACTION MOUSE EVENTS</p>
                            <p>GLOBAL ACTION MOUSE EVENTS</p>
                            <p>GLOBAL ACTION MOUSE EVENTS</p>
                            <p>GLOBAL ACTION MOUSE EVENTS</p>
                            <p>GLOBAL ACTION MOUSE EVENTS</p>
                            <p>GLOBAL ACTION MOUSE EVENTS</p>
                            <p>GLOBAL ACTION MOUSE EVENTS</p>
                            <p>GLOBAL ACTION MOUSE EVENTS</p>
                            <p>GLOBAL ACTION MOUSE EVENTS</p>
                            <p>GLOBAL ACTION MOUSE EVENTS</p>
                            <p>GLOBAL ACTION MOUSE EVENTS</p>
                            <p>GLOBAL ACTION MOUSE EVENTS</p>
                            <p>GLOBAL ACTION MOUSE EVENTS</p>
                            <p>GLOBAL ACTION MOUSE EVENTS</p>
                            <p>GLOBAL ACTION MOUSE EVENTS</p>
                            <p>GLOBAL ACTION MOUSE EVENTS</p>
                            <p>GLOBAL ACTION MOUSE EVENTS</p>
                            <p>GLOBAL ACTION MOUSE EVENTS</p>
                            <p>GLOBAL ACTION MOUSE EVENTS</p>
                            <p>GLOBAL ACTION MOUSE EVENTS</p>
                            <p>GLOBAL ACTION MOUSE EVENTS</p>
                        </div>
                        <div class="tab-pane" id="tabGlobalActionsLoadUnload">
                            GLOBAL ACTION LOAD/UNLOAD
                        </div>
                        <div class="tab-pane" id="tabGlobalActionsWindowEvents">
                            GLOBAL ACTION WINDOW EVENTS
                        </div>
                    </div>
                </div>`;
    }   

    Load() {

    }
    
    PostRender() {
        this.ActionList.PostRender();
    }    
}

class SectionApplications {
    constructor(applications) {
        this.Applications = applications;
        this.ApplicationList = new ApplicationList(this.Applications);
        this.ActionList = this.ApplicationList.ActionList;
        this.Load();
    }

    get CurrentApplication() {
        log('SectionApplications:CurrentApplication');
        return this.ApplicationList.CurrentApplication;
    }

    get CurrentActionItem() {
        log('SectionApplications:CurrentActionItem');
        return this.ApplicationList.CurrentAction;
    }

    get HTML() {
        return this.ApplicationList.HTML;
    }

    Load() {
        log('SectionApplications:Load');
    }

    PostRender() {
        log('SectionApplications:PostRender');
        this.ApplicationList.PostRender();
    }
}

/*********************************************************************************
 *  NAVIGATION
 *********************************************************************************/

function navLoadSection() {
    let section = $('.sidebar ul li').find('.active').prop('id');
    if(section === "ToggleSidebar") {
        return;
    } else {
        navSaveAllVisibleSections();
        navLoadSectionData(section);

        /*
        settingsResizeElements();
        if($('.sidebar ul li').find('.active').prop('id') == 'SectionWebsite') {
            $('#websiteFrame').removeClass('d-none');
            $('#mainContent').addClass('d-none');
        } else {
            $('#websiteFrame').addClass('d-none');
            $('#mainContent').removeClass('d-none');
        }   
        */ 
    }
}

function navLoadSectionData(section) {

    $('.sectionContainer').css('visibility', 'hidden');
    //$('.sectionContainer').hide();
    $('.sectionContainer').css('max-height', 0);
    var containerElement;

    switch(section) {
        case "SectionGlobalActions":
            containerElement = "containerGlobalActions";
            if(!window.sectionGlobalActions) {
                window.sectionGlobalActions = new SectionGlobalActions(appSettings.GlobalApplication);
                window.currentSection = window.sectionGlobalActions;
                $('#' + containerElement).html(window.sectionGlobalActions.HTML);
                window.sectionGlobalActions.PostRender();                
            } else {
                window.currentSection = window.sectionGlobalActions;
            }
            $('#' + containerElement).css('max-height', '');
            $('#' + containerElement).css('visibility', '');
            //$('#containerGlobalActions').show();
            break;
        case "SectionApplications":
            containerElement = "containerApplications";
            if(!window.sectionApplications) {
                window.sectionApplications = new SectionApplications(appSettings.Applications);
                window.currentSection = window.sectionApplications;
                $('#' + containerElement).html(window.sectionApplications.HTML);
                window.sectionApplications.PostRender();                
            } else {
                window.currentSection = window.sectionApplications;
            }
            $('#' + containerElement).css('max-height', '');
            $('#' + containerElement).css('visibility', '');
            //$('#containerApplications').show();
            break;
    }

    //settingsResizeElements();
    setTimeout(function() { settingsResizeElements();}, 0);
    scrollToTop(containerElement);
}

function navSaveAllVisibleSections() {
    log('navSaveAllVisibleSections(): save all visible sections');
}

function navSetTabsWidth() {
    log('navSetTabsWidth()');

    $('.sectionContainer').each(function() {
        
        var mainTabs = $(this).find('.nav-tabs').first();
        $(mainTabs).width("auto");
        // TODO: Still can see issues with scroll bars when resizing
        //       Changing sections and coming back slowly resolves 
        //       Figure out proper solution
        //       Seems like it's due to the resize action not being complete
        //       when getting the tab-content width - so like a sleep
        //       and await ui update would probably resolve
        var mainTabContent = mainTabs.next('.tab-content').first();
        $(mainTabs).width($(mainTabContent).width());
        if($(this).css("visibility") !== "hidden") {
            $(mainTabContent).css('padding-top',($(mainTabs).height()+1)+'px')
        }
    });
}

function navToggleSidebar() {
    if(window.currentSection && window.currentSection.ActionList && window.currentSection.ActionList.ActionEditor && window.currentSection.ActionList.ActionEditor.StepsScriptEditor.ScriptEditor.CodeMirror) {
        window.currentSection.ActionList.ActionEditor.StepsScriptEditor.ScriptEditor.CodeMirror.setSize(100, 100);
    }
    if($('.sidebar-menu-item').hasClass('d-none')) {
        $('.sidebar-menu-item').removeClass('d-none');
        $('#ToggleSidebar .icon-tabler-chevrons-right').addClass('d-none');
        $('#ToggleSidebar .icon-tabler-chevrons-left').removeClass('d-none');
    } else {
        $('.sidebar-menu-item').addClass('d-none')
        $('#ToggleSidebar .icon-tabler-chevrons-left').addClass('d-none');
        $('#ToggleSidebar .icon-tabler-chevrons-right').removeClass('d-none');
    }
    setTimeout(function() { settingsResizeElements();}, 0);
    log('navToggleSidebar(): Remember state of sidebar toggle');
    appSettings.SettingsSidebarCollapsed = !appSettings.SettingsSidebarCollapsed;
 
}

