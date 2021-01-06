var newsportalApp = angular.module('newsportalApp', ['ngResource']);
//FACTORY
newsportalApp.factory('Topic', ['$resource', function ($resource) {
    return $resource('', {}, {
        query: {
            method: 'GET',
            url: '/Home/GetCategoryData',
            params: { format: 'json' },
            isArray: false
        },
        firstQuery: {
            method: 'GET',
            url: '/Home/GetTopic',
            params: { format: 'json' },
            isArray: false
        },
        sectionQuery: {
            method: 'GET',
            url: '/Home/GetResultSection',
            params: { format: 'json' },
            isArray: false
        }
    });
}]);
function selectDate(date) {
    $('.calendar-wrapper').updateCalendarOptions({
        date: date
    });
}

newsportalApp.controller('NewsCtrl', ['$scope', '$window', 'Topic',
    function ($scope, $window, Topic) {
        $scope.refresh = function () {
            $rootScope.$emit('lazyImg:refresh');
        };
        $('.calendar-wrapper').calendar({
            weekDayLength: 1,
            date: new Date(),
            onClickDate: selectDate,
            showYearDropdown: true
        });
        $scope.isSearching = false;
        $scope.page = 0;
        $scope.pagesCount = 0;
        var urlPath = window.location.pathname;
        $scope.url = window.location.origin;
        $scope.canonialurl = $scope.url + urlPath;
        var topic = "";
        if (urlPath !== "" || urlPath !== null) {
            var queryData = urlPath.split('/');
            topic = queryData[queryData.length - 1];
            $scope.categ = queryData[queryData.length - 1];
            $scope.title = $scope.categ;
        }
        $scope.dataLoad = function () {
            Topic.firstQuery({ topic: topic }).$promise.then(function (data) {
                if (data !== null) {
                    var totcount = parseInt(JSON.parse(data.count)[0]["TotalCount"]);
                    $scope.page = 0;
                    $scope.pagesCount = parseInt(Math.ceil(totcount / 10));
                    $scope.totalCount = totcount;
                    $scope.Data = {};
                    if (data.data !== null) {
                        $scope.Data = JSON.parse(data.data);
                    }
                    $scope.PopularData = JSON.parse(data.populardata);
                    $scope.LatestData = JSON.parse(data.latestdata);
                    $scope.isSearching = false;
                    loaderHide();
                }

            }, function (error) {
                $scope.isSearching = false;
            });
        };
        $scope.dataLoad();
        $scope.latestPage = 0;
        $scope.popularPage = 0;
        $scope.lastLatestData = null;
        $scope.lastPopularData = null;
        $scope.LoadMore = function (section) {
            loaderShow();
            if (section === 'Latest') {
                $scope.latestPage = $scope.latestPage + 1;
                Topic.sectionQuery({
                    categoryId: 0, pageIndex: $scope.popularPage, section: 'Latest'
                }).$promise.then(function (data) {
                    $scope.btnHide('Latest', JSON.parse(data.data));
                    loaderHide();
                }, function (error) {
                });
            }
            else if (section === 'Popular') {
                $scope.popularPage = $scope.popularPage + 1;
                Topic.sectionQuery({
                    categoryId: 0, pageIndex: $scope.popularPage, section: 'Popular'
                }).$promise.then(function (data) {
                    $scope.btnHide('Popular', JSON.parse(data.data));
                    loaderHide();
                }, function (error) {
                });
            }

        };
        $scope.btnHide = function (btn_name, data) {
            if (btn_name === "Latest") {
                if (data.length !== 0) {
                    $scope.btnlatest = false;
                    $scope.LatestData = data;
                    $scope.lastLatestData = $scope.LatestData;
                    if (data.length < 6) {
                        $scope.btnlatest = true;
                    }
                }
                else {
                    $scope.btnlatest = true;
                    if ($scope.lastPhtoData !== null) {
                        $scope.lastPopularData = $scope.lastLatestData;
                    }
                }
            }
            else {
                if (data.length !== 0) {
                    $scope.btnpopular = false;
                    $scope.PopularData = data;
                    $scope.lastPopularData = $scope.PopularData;
                    if (data.length < 6) {
                        $scope.btnpopular = true;
                    }
                }
                else {
                    $scope.btnpopular = true;
                    if ($scope.lastPopularData !== null) {
                        $scope.PopularData = $scope.lastPopularData;
                    }
                }

            }
        };
        $scope.search = function (page) {
            loaderShow();
            page = page || 0;
            $scope.isSearching = true;
            Topic.query({ page: page, pageSize: 10, categoryId: categoryId }).$promise.then(function (data) {
                $scope.page = data.CurrentPages;
                $scope.pagesCount = data.TotalPages;
                $scope.totalCount = data.totalCount;
                $scope.Data = {};
                $scope.Data = data.news;
                $scope.isSearching = false;
                loaderHide();
            }, function (error) {
                $scope.isSearching = false;
            });
            $scope.isSearching = true;
        };
    }]
);
// DIRECTIVES
newsportalApp.directive('newsportalPager', function () {
    return {
        scope: {
            page: '@',
            pagesCount: '@',
            totalCount: '@',
            searchFunc: '&'
        },
        replace: true,
        restrict: 'E',
        templateUrl: '../../../Scripts/templates/pager-template.html',
        controller: ['$scope', function ($scope) {
            $scope.search = function (i) {
                if ($scope.searchFunc) {
                    $scope.searchFunc({ page: i });
                }
            };
            $scope.range = function () {
                if (!$scope.pagesCount) { return []; }
                var step = 2;
                var doubleStep = step * 2;
                var start = Math.max(0, $scope.page - step);
                var end = start + 1 + doubleStep;
                if (end > parseInt($scope.pagesCount)) { end = parseInt($scope.pagesCount); }
                var ret = [];
                for (var i = start; i !== end; ++i) {
                    ret.push(i);
                }

                return ret;
            };
        }]
    };
});
newsportalApp.directive('list', function () {
    return {
        restrict: 'E',
        scope: {
            data: '=',
            mainurl: '@'
        },
        templateUrl: '../../../Scripts/templates/listView-template.html'
    };
});
newsportalApp.directive('latestlist', function () {
    return {
        restrict: 'E',
        scope: {
            data: '=',
            mainurl: '@'
        },
        templateUrl: '../../../Scripts/templates/other-template.html'
    };
});
newsportalApp.directive('popularlist', function () {
    return {
        restrict: 'E',
        scope: {
            data: '=',
            mainurl: '@'
        },
        templateUrl: '../../../Scripts/templates/other-template.html'
    };
});
newsportalApp.directive(
    "bnLazySrc", ['$window', '$document',
        function ($window, $document) {


            // I manage all the images that are currently being
            // monitored on the page for lazy loading.
            var lazyLoader = (function () {

                // I maintain a list of images that lazy-loading
                // and have yet to be rendered.
                var images = [];

                // I define the render timer for the lazy loading
                // images to that the DOM-querying (for offsets)
                // is chunked in groups.
                var renderTimer = null;
                var renderDelay = 100;

                // I cache the window element as a jQuery reference.
                var win = $($window);

                // I cache the document document height so that
                // we can respond to changes in the height due to
                // dynamic content.
                var doc = $document;
                var documentHeight = doc.height();
                var documentTimer = null;
                var documentDelay = 2000;

                // I determine if the window dimension events
                // (ie. resize, scroll) are currenlty being
                // monitored for changes.
                var isWatchingWindow = false;


                // ---
                // PUBLIC METHODS.
                // ---


                // I start monitoring the given image for visibility
                // and then render it when necessary.
                function addImage(image) {

                    images.push(image);

                    if (!renderTimer) {

                        startRenderTimer();

                    }

                    if (!isWatchingWindow) {

                        startWatchingWindow();

                    }

                }


                // I remove the given image from the render queue.
                function removeImage(image) {

                    // Remove the given image from the render queue.
                    for (var i = 0; i < images.length; i++) {

                        if (images[i] === image) {

                            images.splice(i, 1);
                            break;

                        }

                    }

                    // If removing the given image has cleared the
                    // render queue, then we can stop monitoring
                    // the window and the image queue.
                    if (!images.length) {

                        clearRenderTimer();

                        stopWatchingWindow();

                    }

                }


                // ---
                // PRIVATE METHODS.
                // ---


                // I check the document height to see if it's changed.
                function checkDocumentHeight() {

                    // If the render time is currently active, then
                    // don't bother getting the document height -
                    // it won't actually do anything.
                    if (renderTimer) {

                        return;

                    }

                    var currentDocumentHeight = doc.height();

                    // If the height has not changed, then ignore -
                    // no more images could have come into view.
                    if (currentDocumentHeight === documentHeight) {

                        return;

                    }

                    // Cache the new document height.
                    documentHeight = currentDocumentHeight;

                    startRenderTimer();

                }


                // I check the lazy-load images that have yet to
                // be rendered.
                function checkImages() {

                    // Log here so we can see how often this
                    // gets called during page activity.

                    var visible = [];
                    var hidden = [];

                    // Determine the window dimensions.
                    var windowHeight = win.height();
                    var scrollTop = win.scrollTop();

                    // Calculate the viewport offsets.
                    var topFoldOffset = scrollTop;
                    var bottomFoldOffset = (topFoldOffset + windowHeight);

                    // Query the DOM for layout and seperate the
                    // images into two different categories: those
                    // that are now in the viewport and those that
                    // still remain hidden.
                    for (var i = 0; i < images.length; i++) {

                        var image = images[i];

                        if (image.isVisible(topFoldOffset, bottomFoldOffset)) {

                            visible.push(image);

                        } else {

                            hidden.push(image);

                        }

                    }

                    // Update the DOM with new image source values.
                    for (var j = 0; j < visible.length; j++) {

                        visible[j].render();

                    }

                    // Keep the still-hidden images as the new
                    // image queue to be monitored.
                    images = hidden;

                    // Clear the render timer so that it can be set
                    // again in response to window changes.
                    clearRenderTimer();

                    // If we've rendered all the images, then stop
                    // monitoring the window for changes.
                    if (!images.length) {

                        stopWatchingWindow();

                    }

                }


                // I clear the render timer so that we can easily
                // check to see if the timer is running.
                function clearRenderTimer() {

                    clearTimeout(renderTimer);

                    renderTimer = null;

                }


                // I start the render time, allowing more images to
                // be added to the images queue before the render
                // action is executed.
                function startRenderTimer() {

                    renderTimer = setTimeout(checkImages, renderDelay);

                }


                // I start watching the window for changes in dimension.
                function startWatchingWindow() {

                    isWatchingWindow = true;

                    // Listen for window changes.
                    win.on("resize.bnLazySrc", windowChanged);
                    win.on("scroll.bnLazySrc", windowChanged);

                    // Set up a timer to watch for document-height changes.
                    documentTimer = setInterval(checkDocumentHeight, documentDelay);

                }


                // I stop watching the window for changes in dimension.
                function stopWatchingWindow() {

                    isWatchingWindow = false;

                    // Stop watching for window changes.
                    win.off("resize.bnLazySrc");
                    win.off("scroll.bnLazySrc");

                    // Stop watching for document changes.
                    clearInterval(documentTimer);

                }


                // I start the render time if the window changes.
                function windowChanged() {

                    if (!renderTimer) {

                        startRenderTimer();

                    }

                }


                // Return the public API.
                return ({
                    addImage: addImage,
                    removeImage: removeImage
                });

            })();


            // ------------------------------------------ //
            // ------------------------------------------ //


            // I represent a single lazy-load image.
            function LazyImage(element) {

                // I am the interpolated LAZY SRC attribute of
                // the image as reported by AngularJS.
                var source = null;

                // I determine if the image has already been
                // rendered (ie, that it has been exposed to the
                // viewport and the source had been loaded).
                var isRendered = false;

                // I am the cached height of the element. We are
                // going to assume that the image doesn't change
                // height over time.
                var height = null;


                // ---
                // PUBLIC METHODS.
                // ---


                // I determine if the element is above the given
                // fold of the page.
                function isVisible(topFoldOffset, bottomFoldOffset) {

                    // If the element is not visible because it
                    // is hidden, don't bother testing it.
                    if (!element.is(":visible")) {

                        return (false);

                    }

                    // If the height has not yet been calculated,
                    // the cache it for the duration of the page.
                    if (height === null) {

                        height = element.height();

                    }

                    // Update the dimensions of the element.
                    var top = element.offset().top;
                    var bottom = (top + height);

                    // Return true if the element is:
                    // 1. The top offset is in view.
                    // 2. The bottom offset is in view.
                    // 3. The element is overlapping the viewport.
                    return (
                        (
                            (top <= bottomFoldOffset) &&
                            (top >= topFoldOffset)
                        )
                        ||
                        (
                            (bottom <= bottomFoldOffset) &&
                            (bottom >= topFoldOffset)
                        )
                        ||
                        (
                            (top <= topFoldOffset) &&
                            (bottom >= bottomFoldOffset)
                        )
                    );

                }


                // I move the cached source into the live source.
                function render() {

                    isRendered = true;

                    renderSource();

                }


                // I set the interpolated source value reported
                // by the directive / AngularJS.
                function setSource(newSource) {

                    source = newSource;

                    if (isRendered) {

                        renderSource();

                    }

                }


                // ---
                // PRIVATE METHODS.
                // ---


                // I load the lazy source value into the actual
                // source value of the image element.
                function renderSource() {

                    element[0].src = source;

                }


                // Return the public API.
                return ({
                    isVisible: isVisible,
                    render: render,
                    setSource: setSource
                });

            }


            // ------------------------------------------ //
            // ------------------------------------------ //


            // I bind the UI events to the scope.
            function link($scope, element, attributes) {

                var lazyImage = new LazyImage(element);

                // Start watching the image for changes in its
                // visibility.
                lazyLoader.addImage(lazyImage);


                // Since the lazy-src will likely need some sort
                // of string interpolation, we don't want to
                attributes.$observe(
                    "bnLazySrc",
                    function (newSource) {

                        lazyImage.setSource(newSource);

                    }
                );


                // When the scope is destroyed, we need to remove
                // the image from the render queue.
                $scope.$on(
                    "$destroy",
                    function () {

                        lazyLoader.removeImage(lazyImage);

                    }
                );

            }


            // Return the directive configuration.
            return ({
                link: link,
                restrict: "A"
            });

        }
    ]);