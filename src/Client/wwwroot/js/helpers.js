"use strict";

function initHelpers() {
  var helpers = {};

  // clickedOutside begin
  var clickedOutsideListeners = {}

  helpers.clickedOutsideAddListener = function (dotnetHelper, method, selector) {
    var listener = function (event) {
      if ($(event.target).parents(selector).length === 0) {
        dotnetHelper.invokeMethodAsync(method);
      }
    }
    if (!clickedOutsideListeners.hasOwnProperty(selector)) {
      clickedOutsideListeners[selector] = listener;
      $(document).on('click', listener);
    }
  }

  helpers.clickedOutsideRemoveListener = function (selector) {
    if (clickedOutsideListeners.hasOwnProperty(selector)) {
      $(document).off('click', clickedOutsideListeners[selector]);
      delete clickedOutsideListeners[selector];
    }
  }
  // clickedOutside end

  helpers.focus = function (selector) {
    $(selector).focus();
  }
  
  helpers.inputClear = function (selector) {
    $(selector).val("");
  }

  return helpers;
}

window.helpers = initHelpers();
