'use strict';
var hideMenu = true;
// var app = angular.module( 'puridiomSeedApp', [] );

var app = angular.module( 'puridiomSeedApp', [
    'ngCookies',
    'ngResource',
    'ngSanitize',
    'ngRoute',
    'ngAnimate',
    'ngTouch',
    'mgcrea.ngStrap',
    'ngStorage'
  ] )
  .config( function ( $routeProvider ) {
    $routeProvider
      .when('/', {
    	  controller: 'LoginController',
  		  templateUrl: 'views/login.html',
  		  serviceId: 'DashboardService'
      })
      .when( '/MultiAction', {
	      controller: 'MainCtrl',
	      templateUrl: 'views/dashboardAngularTest.html',
	      resolve: {resolveData: 'MultiProcessService'}
      } )
      .when( '/RequisitionGeneral', {
		controller: 'RequisitionController',
		templateUrl: 'views/REQP_GENERAL.html'
      } )
      .when( '/Dashboard', {
		controller: 'DesignController',
		templateUrl: 'views/dashboard.html'
      } )
      .when( '/Review', {
		controller: 'DesignController',
		templateUrl: 'views/review.html'
      } )
      .when( '/twoCol', {
         controller: 'DesignController',
         templateUrl: 'views/2col.html'
      } )
      .when( '/CatalogSearch', {
    	  controller: 'MainCtrl',
	      templateUrl: 'views/catalogSearch_directive.html',
	      resolve: {resolveData: 'CatalogService'}
      } )
      .when( '/CatalogSearchStatic', {
    	  controller: 'MainCtrl',
	      templateUrl: 'views/catalogSearch_categories.html',
	      //resolve: {resolveData: 'CatalogService'}
      } )
      .when( '/CheckOut/:processKey', {
    	  controller: 'MainCtrl',
          templateUrl: 'views/dashboardAngularTest.html',
          resolve: {resolveData: 'CheckOutService'}
      } )
      .when( '/ShoppingCart', {
    	  controller: 'ShoppingCartController',
	      templateUrl: 'views/shoppingCart.html',
	      //resolve: {resolveData: 'CheckOutService'}
      } )
      .when( '/CatalogSearchStatic', {
    	  controller: 'MainCtrl',
	      templateUrl: 'views/catalogSearch_categories.html',
	      //resolve: {resolveData: 'CatalogService'}
      } )
      .when( '/ShoppingCart', {
    	  controller: 'ShoppingCartController',
	      templateUrl: 'views/shoppingCart.html',
	      //resolve: {resolveData: 'CheckOutService'}
      } )
      .when( '/CatalogSearch_viewAll/:browseId/:pageSize/:pageCount', {
         controller: 'MainCtrl',
         templateUrl: 'views/catalogSearchViewAll_directive.html',
   	     resolve: {resolveData: 'CatalogViewAllService'}
      } )
      .when( '/CatalogSearch_viewDetails', {
         controller: 'DesignController',
         templateUrl: 'views/catalogSearch_viewDetails.html'
      } )
      .when( '/CatalogSearch_itemCompare', {
         controller: 'DesignController',
         templateUrl: 'views/catalogSearch_itemCompare.html'
      } )
      .when( '/Register/:module/:service/:pageId/', {
    	controller: 'MainCtrl',
      	templateUrl: 'views/marketPlace.html',
    	resolve: {resolveData: 'RegisterService'}
      } )
      .when( '/RegisterOption', {
      controller: 'DesignController',
      templateUrl: 'views/register-option.html'
      } )
      .when( '/RegisterComplete', {
      controller: 'DesignController',
      templateUrl: 'views/register-complete.html'
      } )
      .when( '/RegisterEnterKey', {
      controller: 'DesignController',
      templateUrl: 'views/register-enter.html'
      } )
      .when( '/DashboardTiles/:pageId/:browseKey', {
      controller: 'MainCtrl',
      templateUrl: 'views/dashboardAngularTest.html',
      resolve: {resolveData: 'DashboardTilesService'}
      } )
      .when( '/question/:pageId/:type', {
	      controller: 'MainCtrl',
	      templateUrl: 'views/questionTemplate.html',
	      resolve: {resolveData: 'QuestionService'}
      } )
      .when( '/ReqCreateTiles/:pageId/:browseKey', {
      controller: 'MainCtrl',
      templateUrl: 'views/dashboardAngularTest.html',
      resolve: {resolveData: 'DashboardTilesService'}
      } )
      .when( '/DashboardCeltson', {
      controller: 'MainCtrl',
      templateUrl: 'views/dashboardAngularTest.html',
      resolve: {resolveData: 'DashboardService'}
      } )
      .when( '/MarketPlaceDashboard', {
      controller: 'MainCtrl',
      templateUrl: 'views/dashboardAngularTest.html',
      resolve: {resolveData: 'MarketPlaceDashboardService'}
      } )
      .when( '/createRequisition/:module/:service/:pageId/:type', {
	      controller: 'MainCtrl',
	      templateUrl: 'views/dashboardAngularTest.html',
	      resolve: {resolveData: 'RequisitionService'}
      } )
      .when( '/openRequisition/:icReqHeader', {
      controller: 'MainCtrl',
      templateUrl: 'views/dashboardAngularTest.html',
      resolve: {resolveData: 'ReviewFinalizeService'}
      } )
      .when( '/openOrder/:icPoHeader', {
      controller: 'MainCtrl',
      templateUrl: 'views/dashboardAngularTest.html',
      resolve: {resolveData: 'PoReviewFinalizeService'}
      } )
      .when( '/openApprovalPage/:icReqHeader', {
	      controller: 'MainCtrl',
	      templateUrl: 'views/dashboardAngularTest.html',
	      resolve: {resolveData: 'ApprovalService'}
      } )
      .when( '/open/:module/:service/:pageId/:icReqHeader', {
	      controller: 'MainCtrl',
	      templateUrl: 'views/dashboardAngularTest.html',
	      resolve: {resolveData: 'TwoColService'}
      } )
      .when( '/open/:module/:service/:pageId/:browseId/:icReqHeader', {
	      controller: 'MainCtrl',
	      templateUrl: 'views/dashboardAngularTest.html',
	      resolve: {resolveData: 'TwoColService'}
      } )
      .when( '/open/:module/:service/:pageId/:icReqHeader/:icReqLine', {
	      controller: 'MainCtrl',
	      templateUrl: 'views/dashboardAngularTest.html',
	      resolve: {resolveData: 'TwoColService'}
      } )
      .when( '/openLine/:module/:service/:pageId/:icReqLine/:icReqHeader', {
	      controller: 'MainCtrl',
	      templateUrl: 'views/addItem_view.html',
	      resolve: {resolveData: 'TwoColLineService'}
      } )
      .when( '/openLineBrowse/:module/:service/:pageId/:browseId/:icReqLine/:icReqHeader', {
	      controller: 'MainCtrl',
	      templateUrl: 'views/addItem_view.html',
	      resolve: {resolveData: 'TwoColLineService'}
      } )
      .when( '/openLineBrowse/:module/:service/:pageId/:browseId/:icReqHeader', {
	      controller: 'MainCtrl',
	      templateUrl: 'views/addItem_view.html',
	      resolve: {resolveData: 'TwoColLineService'}
      } )
      .when( '/openLineStep/:module/:service/:pageId/:icReqLine/:icReqHeader', {
	      controller: 'MainCtrl',
	      templateUrl: 'views/dashboardAngularTest.html',
	      resolve: {resolveData: 'TwoColLineService'}
      } )
      .when( '/openLineAccount/:module/:service/:pageId/:icReqLine/:icReqHeader', {
	      controller: 'MainCtrl',
	      templateUrl: 'views/dashboardAngularTest.html',
	      resolve: {resolveData: 'AccountService'}
      } )
      .when( '/openLine/:module/:service/:pageId/:browseId/:icReqLine/:icReqHeader', {
	      controller: 'MainCtrl',
	      templateUrl: 'views/dashboardAngularTest.html',
	      resolve: {resolveData: 'TwoColLineService'}
      } )
      .when( '/openLine/:module/:service/:pageId/:icReqHeader', {
	      controller: 'MainCtrl',
	      templateUrl: 'views/addItem_view.html',
	      resolve: {resolveData: 'TwoColLineService'}
      } )
      .when( '/openRouting/:module/:service/:pageId/:icReqHeader', {
	      controller: 'MainCtrl',
	      templateUrl: 'views/dashboardAngularTest.html',
	      resolve: {resolveData: 'RoutingService'}
      } )
      .when( '/User/:userId/:organizationId', {
         controller: 'MainCtrl',
         templateUrl: 'views/userProfile.html',
         resolve: {resolveData: 'UserProfileService'}
      } )
      .when( '/User', {
         controller: 'MainCtrl',
         templateUrl: 'views/userProfileView.html',
         resolve: {resolveData: 'UserProfileViewService'}
      } )
      .when( '/openBrowse/:pageId/:browseId', {
         controller: 'MainCtrl',
         templateUrl: 'views/dashboardAngularTest.html',
         resolve: {resolveData: 'BrowseOpenService'}
      } )
      .when( '/Accounts_edit_test', {
         controller: 'DesignController',
         templateUrl: 'views/accounts_edit_test.html'
      } )
      .when( '/DashboardBrowse', {
         controller: 'DesignController',
         templateUrl: 'views/dashboard_browseView_Details.html'
      } )
      .when( '/DashboardTiles_withNew', {
         controller: 'DesignController',
         templateUrl: 'views/dashboardTiles.html'
      } )
      .when( '/DashboardTiles_withNew_createNew', {
         controller: 'DesignController',
         templateUrl: 'views/dashboardTiles_createNew.html'
      } )
      .when( '/SwipeTest', {
         controller: 'DesignController',
         templateUrl: 'views/dashboardTiles_2.html'
      } )
      .when( '/Routing', {
         controller: 'DesignController',
         templateUrl: 'views/routingList.html'
      } )
      .when( '/AdminCheck', {
         controller: 'DesignController',
         templateUrl: 'views/adminCheck.html'
      } )
      .when( '/Comments', {
         controller: 'DesignController',
         templateUrl: 'views/comments.html'
      } )
      .when( '/Attachments', {
         controller: 'DesignController',
         templateUrl: 'views/attachments.html'
      } )
      .when( '/Approval', {
         controller: 'DesignController',
         templateUrl: 'views/approvals.html'
      } )
      .when( '/Confirmation', {
         controller: 'DesignController',
         templateUrl: 'views/confirmation.html'
      } )
      .when( '/AddItem', {
         controller: 'DesignController',
         templateUrl: 'views/addItem.html'
      } )
      .when( '/ManageServer', {
    	  controller: 'MainCtrl',
    	  templateUrl: 'views/management.html',
    	  resolve: {resolveData: 'ManagementService'}
      } )
      .when( '/Error', {
    	  controller: 'ErrorController',
    	  templateUrl: 'views/error.html'
      } )
      .when( '/AdminSetupStep', {
         controller: 'DesignController',
         templateUrl: 'views/adminSetup.html'
      } )
      .when( '/AdminSetupProcess', {
         controller: 'DesignController',
         templateUrl: 'views/adminSetup_Process.html'
      } )
      .when('/Logout', {
    	  controller: 'LogoutController',
  		  templateUrl: 'views/login.html',
  		  resolve: {resolveData: 'LogoutService'}
      })
      .otherwise( {
         redirectTo: '/'
      } );
  } );

//MyService will contain all parameters we need
app.factory("PuridiomData", function() {
	// This service I create this for pass parameters from controller to controller
	return {
		data: {},
		request: {
				requestHeader: {
					pageId : "",
					moduleRequest: "",
					module: "",
					service: "",
					processKey: "",
					browseKey: "",
					identity: {
						organizationId: "CINCO",
						userId: "",
						mailId: "",
						sharedSecret: "",
						language: "EN"
					}
				},
				requestBody: {}
		},
		userProfile: {
			fullName: ''
		}
	};
});

//MyService will contain all parameters we need
app.factory("ContainerData", function() {
	// This service I create this for pass parameters from controller to controller
	return {
		response: {},
		catalog: [],
		filters: []
	};
});

app.factory("PurdiomConfig", function() {
	// This service is for pass the URL
	return {
		puridiomscurl: '/puridiom5/procure'
	};
});

app.factory('SharedService', function($rootScope) {
	//this service is for pass events from controller to controller
    var sharedService = {
    		hideMenu : false,
    		startMessageAlert : true
    };

    sharedService.isHideMenu = function(isHideMenu) {
        sharedService.hideMenu = isHideMenu;
        $rootScope.$broadcast('eventHideMenu');
    };
    
    sharedService.runMessageAlertService = function(){
    	$rootScope.$broadcast('eventMessageAlertService');
    };
    return sharedService;
});

///http://localhost:8080/puridiom5/procure
//mark@puridiom.com / WELCOME@1
