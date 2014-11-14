//------------------------AplicationName, dependecias
var app = angular.module('appDemo', ['ngRoute', 'ngCookies',
    'ngResource',
    'ngSanitize',
    'ngAnimate',
    'ngTouch'])

    .config(['$routeProvider', function($routeProvider) {
        $routeProvider
        .when("/", {
            controller: "LoginController",
            templateUrl: "login.htm"
        })
        .when("/index", {
            controller: "LoginController",
            templateUrl: "login.htm"
        })
        .when('/demo', {
            controller: 'DemoController',
            templateUrl: 'home.htm'
        })
        .when('/register', {
            templateUrl: '/Registration.aspx/RegisterUser'
        })
        .when("/logout", {
            controller: "LoginController",
            templateUrl: "login.htm"
        })
        .otherwise({ redirect: '/' });
    } ])
    ;
    app.controller('LoginController', ['$rootScope', '$scope', '$location', 'CompelligenceHTTP', function($rootScope, $scope, $location, CompelligenceHTTP) {
        console.log('LOGIN CONTROLLER');
        $rootScope.showCompanyLogo = false;
        $rootScope.showOptionMenu = false;
        $rootScope.showBackEndTile = false;
        $scope.username = 'compelligence_companyb@yahoo.com.pe';
        $scope.kennwort = 'password';
        $scope.showCompanyLogo = false;
        $scope.windowLocation = function() {
            //window.location.href = '/Registration.aspx/RegisterUser';
            $window.location.href = '/Registration.aspx/RegisterUser';
            location.href = '/Registration.aspx/RegisterUser';
        };
        $scope.login = function() {
            if ($scope.username != '' && $scope.kennwort != '') {
                console.log('login function');
                //$location.path("/demo");
            }
            CompelligenceHTTP.sendMessage($scope)
				.then(function(response) {
				    console.log("Response:" + response);
				    if (response != null) {
				        if (response.user != null) {
				            if (response.user.status === 'ENBL') {
				                console.log(response.user.logoClient);
				                if (response.user.logoClient != undefined && response.user.logoClient != null && response.user.logoClient != '') {
				                    $rootScope.showCompanyLogo = true;
				                    $rootScope.logoClient = response.user.logoClient;
				                }
				                if (response.user.securityGroup != 'ENDUSER') {
				                    $rootScope.showBackEndTile = true;
				                }
				                $rootScope.showOptionMenu = true;
				                $location.path("/demo");
				            } else {
				                $scope.errorsLogin = true;
				                $scope.errorsMessage = "User status is disabled";
				            }
				        } else {
				            $scope.errorsLogin = true;
				            $scope.errorsMessage = "The email or password provided is incorrect";
				        }
				    } else {
				        $scope.errorsLogin = true;
				        $scope.errorsMessage = "The email or password provided is incorrect";
				    }

				}
            //			      , function(response) {
            //			    	  console.log(response);
            //			    	  //$scope.load = false;
            //			    	  $location.path("/demo");
            //			   }
			      );
        };
    } ]);
app.controller('HeaderController', function($scope) {

});
app.controller('DemoController', ['$rootScope', '$scope', function($rootScope, $scope) {
$scope.tiles = tiles;
//$rootScope.showCompanyLogo = true;
}]);
var tiles = [
        {
            title: "Generate a competitive sales stategy",
            link: "Comparinator.aspx/Index",
            logo: "./Content/Images/Styles/undefperson.jpg",
            description: ""
        },
        {
            title: "Research Markets & Competitors",
            link: "ContentPortal.aspx/Index",
            logo: "./Content/Images/Styles/undefperson.jpg",
            description: ""
        },
//        {
//            title: "Find Competitive Content",
//            link: "#",
//            logo: "./Content/Images/Styles/undefperson.jpg",
//            description: ""
//        },
//        {
//            title: "Report a Win/Loss",
//            link: "#",
//            logo: "./Content/Images/Styles/undefperson.jpg",
//            description: ""
//        },
//        {
//            title: "Discuss the Competition",
//            link: "#",
//            logo: "./Content/Images/Styles/undefperson.jpg",
//            description: ""
//        },
//        {
//            title: "Report a Competitive Sighting",
//            link: "#",
//            logo: "./Content/Images/Styles/undefperson.jpg",
//            description: ""
//        },
        {
            title: "Back End",
            link: "BackEnd.aspx",
            logo: "./Content/Images/Styles/undefperson.jpg",
            description: ""
        }
    ];
