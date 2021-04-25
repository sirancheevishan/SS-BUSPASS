/*!
 * Validator v0.0.1
 * https://github.com/fengyuanchen/validator
 *
 * Copyright (c) 2015 Fengyuan Chen
 * Released under the MIT license
 *
 * Date: 2015-03-15T07:51:11.245Z
 */


(function (factory) {
    if (typeof define === 'function' && define.amd) {
        // AMD. Register as anonymous module.
        define('validator', ['jquery'], factory);
    } else if (typeof exports === 'object') {
        // Node / CommonJS
        factory(require('jquery'));
    } else {
        // Browser globals.
        factory(jQuery);
    }
})(function ($) {

    'use strict';

    var NAMESPACE = '.validator',
        EVENT_SUCCESS = 'success' + NAMESPACE,
        EVENT_ERROR = 'error' + NAMESPACE,

        Validator = function (element, options) {
            this.element = element;
            this.$element = $(element);
            this.options = $.extend(true, {}, Validator.DEFAULTS, $.isPlainObject(options) && options);
            this.isCheckboxOrRadio = /checkbox|radio/.test(element.type);
            this.value = null;
            this.valid = true;
            this.init();
        },

        prototype = Validator.prototype;

    function isNumber(n) {
        return typeof n === 'number';
    }

    function isString(n) {
        return typeof n === 'string';
    }

    function isUndefined(n) {
        return typeof n === 'undefined';
    }

    function toArray(obj, offset) {
        var args = [];

        if (isNumber(offset)) { // It's necessary for IE8
            args.push(offset);
        }

        return args.slice.apply(obj, args);
    }

    Validator.DEFAULTS = {
        // Type: Object
        /* e.g.: {
          minlength: 8,
          maxlength: 16
        } or {
          rangelength: [8, 16]
        } or {
          rangelength: {
            message: 'Please enter a value between 8 and 16 characters long.',
            validator: function (value) {
              return /^.{8,16}$/.test(value);
            }
          }
        } */
        rules: null,

        // The event(s) which triggers validating
        // Type: String
        trigger: '',

        // Filter the value before validate
        // Type: Function
        filter: null,

        // A shortcut of "success.validator" event
        // Type: Function
        success: null,

        // A shortcut of "error.validator" event
        // Type: Function
        error: null
    };

    Validator.PATTERNS = {
        number: /^-?\d+$/,
        //email: /^[\w.!#$%&'*+\/=?^_`{|}~-]+@[\w](?:[\w-]{0,61}[\w])?(?:\.[\w](?:[\w-]{0,61}[\w])?)*$/,
        email: /^([\w-]+(?:\.[\w-]+)*)@((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-z]{2,6}(?:\.[a-z]{2})?)$/i,

        // By Scott Gonzalez: http://projects.scottsplayground.com/iri/
        url: /^(https?|s?ftp):\/\/(((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:)*@)?(((\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5]))|((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?)(:\d*)?)(\/((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)+(\/(([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)*)*)?)?(\?((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)|[\uE000-\uF8FF]|\/|\?)*)?(#((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)|\/|\?)*)?$/i,

        required: /\S+/
    };

    Validator.MESSAGES = {
        // Types
        number: 'Please enter a valid number (only digits).',
        email: 'Please enter a valid email address.',
        url: 'Please enter a valid URL.',
        date: 'Please enter a valid date.',

        // Attributes
        required: 'This field is required.',
        max: 'Please enter a value less than or equal to [0].',
        min: 'Please enter a value greater than or equal to [0].',
        maxlength: 'Please enter no more than [0] characters.',
        minlength: 'Please enter at least [0] characters.',
        pattern: 'Please enter a matched value.',

        // Customs
        range: 'Please enter a value between [0] and [1].',
        rangelength: 'Please enter a value between [0] and [1] characters long.',
        equalto: 'Please enter the same value again.'
    };

    // validators list
    Validator.VALIDATORS = [
      // Types
      'number',
      'email',
      'url',
      'date',

      // Attributes
      'required',
      'min',
      'max',
      'minlength',
      'maxlength',
      'pattern',

      // Customs
      'range',
      'rangelength',
      'equalto'
    ];

    $.extend(prototype, {
        init: function () {
            this.sync();
            this.bind();
        },

        bind: function () {
            var $this = this.$element,
                options = this.options;

            $this.on(EVENT_SUCCESS, options.success).on(EVENT_ERROR, options.error);

            if (options.trigger) {
                $this.on(options.trigger, $.proxy(this.validate, this));
            }
        },

        unbind: function () {
            var $this = this.$element,
                options = this.options;

            $this.off(EVENT_SUCCESS, options.success).off(EVENT_ERROR, options.error);

            if (options.trigger) {
                $this.off(options.trigger, this.validate);
            }
        }
    });

    $.extend(prototype, {
        // Collects attribute rules
        sync: function () {
            var $this = this.$element,
                options = this.options,
                type = $this.attr('type'),
                validators = Validator.VALIDATORS,
                rules = {};

            if ($.inArray(type, validators) > -1) {
                rules[type] = true;
            }

            $.each(validators, function (i, name) {
                var value = $this.attr(name);

                if (!isUndefined(value)) {
                    switch (name) {
                        case 'min':
                        case 'max':
                        case 'minlength':
                        case 'maxlength':
                            value = Number(value);
                            break;

                        case 'pattern':
                            value = new RegExp(value);
                            break;

                        case 'range':
                        case 'rangelength':
                            value = value.match(/\d+/g);

                            if ($.isArray(value)) {
                                $.map(value, function (n) {
                                    return Number(n);
                                });
                            } else {
                                value = [];
                            }

                            break;
                    }

                    rules[name] = value;
                }

            });

            options.rules = $.extend({}, options.rules, rules);
        },

        addRule: function (name, value) {
            if (isString(name)) {
                this.options.rules[name] = value;
            } else if ($.isPlainObject(name)) {
                $.each(name, $.proxy(this.addRule, this));
            }
        },

        removeRule: function (name) {
            if (isString(name)) {
                delete this.options.rules[name];
            } else if ($.isPlainObject(name)) {
                $.each(name, $.proxy(this.removeRule, this));
            }
        },

        addValidator: function (name, validator) {
            if (isString(name)) {
                if (!this.hasOwnProperty(name) && $.isFunction(validator)) {
                    this[name] = validator;
                    Validator.VALIDATORS.push(name);
                }
            } else if ($.isPlainObject(name)) {
                $.each(name, $.proxy(this.addValidator, this));
            }
        },

        removeValidator: function (name) {
            if (isString(name)) {
                if (this.hasOwnProperty(name) && $.isFunction(this[name])) {
                    delete this[name]; // this[name] = undefined;
                    $.grep(Validator.VALIDATORS, function (n) {
                        return n !== name;
                    });
                }
            } else if ($.isPlainObject(name)) {
                $.each(name, $.proxy(this.removeValidator, this));
            }
        },

        validate: function () {
            var $this = this.$element,
                options = this.options,
                value = $this.val(),
                valid = true,
                rule = {},
                message;

            if (!this.isCheckboxOrRadio && value === this.value) { // Not changed
                return this.valid;
            }

            this.value = value;

            if (options.filter) {
                value = options.filter.call($this[0], value);
            }

            $.each(options.rules, $.proxy(function (type, data) {
                var validator;

                if ($.isPlainObject(data)) {
                    validator = data.validator;
                    message = data.message;
                } else {
                    validator = this[type];
                    message = Validator.MESSAGES[type];
                }

                if ($.isFunction(validator)) {
                    valid = validator.call(this, value, data);
                }

                rule.type = type;
                rule.data = data;

                return valid; // Breaks loop when invalid

            }, this));

            if (valid) {
                message = '';
                $this.trigger($.Event(EVENT_SUCCESS, {
                    message: message,
                    value: value,
                    rule: rule
                }), message);
            } else {
                if (isString(message)) {
                    message = message.replace(/\[\s*(\d+)\s*\]/g, function () {
                        var data = rule.data;
                        return $.isArray(data) ? data[arguments[1]] : data;
                    });
                }

                $this.trigger($.Event(EVENT_ERROR, {
                    message: message,
                    value: value,
                    rule: rule
                }), message);
            }

            this.valid = valid;

            return valid;
        },

        // Alias of valid, as "√"
        v: function () {
            return this.validate();
        },

        // Alias of invalid, as "×"
        x: function () {
            return !this.validate();
        },

        isValid: function () {
            return this.validate();
        },

        isInvalid: function () {
            return !this.validate();
        },

        destroy: function () {
            this.unbind();
            this.$element.removeData('validator');
        }
    });

    $.extend(prototype, {
        number: function (value) {
            return Validator.PATTERNS.number.test(value);
        },

        email: function (value) {
            return Validator.PATTERNS.email.test(value);
        },

        url: function (value) {
            return Validator.PATTERNS.url.test(value);
        },

        date: function (value) {
            return !isNaN(new Date(value).valueOf());
        },

        pattern: function (value, regexp) {
            return $.type(regexp) === 'regexp' && regexp.test(value);
        },

        required: function (value) {
            return this.isCheckboxOrRadio ? this.element.checked : Validator.PATTERNS.required.test(value);
        },

        min: function (value, min) {
            return parseInt(value, 10) >= min;
        },

        max: function (value, max) {
            return parseInt(value, 10) <= max;
        },

        range: function (value, range) {
            value = parseInt(value, 10);

            return $.isArray(range) && range.length === 2 && range[0] <= value && value <= range[1];
        },

        minlength: function (value, min) {
            return String(value).length >= min;
        },

        maxlength: function (value, max) {
            return String(value).length <= max;
        },

        rangelength: function (value, range) {
            var length = String(value).length;

            return $.isArray(range) && range.length === 2 && range[0] <= length && length <= range[1];
        },

        equalto: function (value, target) {
            return value === $(target).val();
        }
    });

    // Save the other validator
    Validator.other = $.fn.validator;

    // Register as jQuery plugin
    $.fn.validator = function (options) {
        var args = toArray(arguments, 1),
            result;

        this.each(function () {
            var $this = $(this),
                data = $this.data('validator'),
                fn;

            if (!data) {
                $this.data('validator', (data = new Validator(this, options)));
            }

            if (isString(options) && $.isFunction((fn = data[options]))) {
                result = fn.apply(data, args);
            }
        });

        return isUndefined(result) ? this : result;
    };

    $.fn.validator.Constructor = Validator;

    $.fn.validator.setDefaults = function (options) {
        $.extend(true, Validator.DEFAULTS, options);
    };

    $.fn.validator.setMessages = function (options) {
        $.extend(Validator.MESSAGES, options);
    };

    $.fn.validator.setValidators = function (options) {
        $.extend(Validator.prototype, options);
    };

    // No conflict
    $.fn.validator.noConflict = function () {
        $.fn.validator = Validator.other;
        return this;
    };

});
