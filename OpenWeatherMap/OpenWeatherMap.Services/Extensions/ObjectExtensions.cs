using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace OpenWeatherMap.Services.Services.Extensions
{
    public static class ObjectExtensions
    {
        /// <summary>
        /// Finds the validator for the given object type.
        /// </summary>
        /// <param name="obj">The object to find a validator for.</param>
        /// <param name="assembly">The assembly to search in (defaults to the assmebly that contains the object type).</param>
        /// <returns>The FluentValidation AbstractValidator type.</returns>
        public static Type FindValidator(this object obj, Assembly? assembly = null)
            => FindValidator(obj.GetType(), assembly);

        /// <summary>
        /// Finds the validator for the given type.
        /// </summary>
        /// <param name="obj">The object to find a validator for.</param>
        /// <param name="assembly">The assembly to search in (defaults to the assmebly that contains the object type).</param>
        /// <returns>The FluentValidation AbstractValidator type.</returns>
        public static Type FindValidator(this Type type, Assembly? assembly = null)
        {
            Type v = typeof(AbstractValidator<>).MakeGenericType(type);

            return (assembly ?? type.Assembly)
                    .GetTypes()
                    .Where(t => t.IsSubclassOf(v))
                    .FirstOrDefault()
                    ?? throw new NullReferenceException($"Could not find validator for {type.Name}");
        }


        // *** SINGLE OBJECT ***

        /// <summary>
        /// Finds an appropiate validator for this object and executes it.
        /// </summary>
        /// <param name="obj">The object to validate.</param>
        /// <exception cref="NullReferenceException"></exception>
        /// <exception cref="ValidationException"></exception>
        public static T Validate<T>(this T obj)
        {
            if (obj is null)
                throw new ArgumentNullException(nameof(obj));

            var validatorType = FindValidator(obj.GetType());
            if (validatorType is null)
                throw new NullReferenceException($"Validator for {obj.GetType().Name} not found");

            Validate(obj, validatorType);

            return obj;
        }

        /// <summary>
        /// Executes the specified validator for this object.
        /// </summary>
        /// <typeparam name="TValidator">The type of validator to use.</typeparam>
        /// <param name="obj">The object to validate.</param>
        /// <exception cref="NullReferenceException"></exception>
        /// <exception cref="ValidationException"></exception>
        public static void Validate<T, TValidator>(this T obj)
            where TValidator : IValidator
            => Validate(obj, typeof(TValidator));

        /// <summary>
        /// Executes the specified validator for this object.
        /// </summary>
        /// <param name="obj">The object to validate.</param>
        /// <param name="validator">The typeof of the validator to use.</param>
        /// <exception cref="NullReferenceException"></exception>
        /// <exception cref="ValidationException"></exception>
        public static void Validate<T>(this T obj, Type validator)
        {
            if (obj is null)
                throw new ArgumentNullException(nameof(obj));

            MethodInfo method = validator
                .GetMethods()
                .Where(m => m.Name.Equals("Validate", StringComparison.OrdinalIgnoreCase))
                .First();

            ValidationResult? result = method.Invoke(Activator.CreateInstance(validator), new object[] { obj }) as ValidationResult;
            if (result is null || !result.IsValid)
                throw new ValidationException(result?.Errors ?? new List<ValidationFailure>());
        }


        /// <summary>
        /// Finds an appropiate validator for this object and executes it.
        /// </summary>
        /// <param name="obj">The object to validate.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <exception cref="NullReferenceException"></exception>
        /// <exception cref="ValidationException"></exception>
        public static Task ValidateAsync(this object obj, CancellationToken cancellationToken = default)
        {
            Validate(obj);
            cancellationToken.ThrowIfCancellationRequested();

            return Task.CompletedTask;
        }

        /// <summary>
        /// Executes the specified validator for this object.
        /// </summary>
        /// <typeparam name="TValidator">The type of validator to use.</typeparam>
        /// <param name="obj">The object to validate.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <exception cref="NullReferenceException"></exception>
        /// <exception cref="ValidationException"></exception>
        public static Task ValidateAsync<TValidator>(this object obj, CancellationToken cancellationToken = default)
            where TValidator : IValidator
        {
            Validate(obj, typeof(TValidator));
            cancellationToken.ThrowIfCancellationRequested();

            return Task.CompletedTask;
        }

        /// <summary>
        /// Executes the specified validator for this object.
        /// </summary>
        /// <param name="obj">The object to validate.</param>
        /// <param name="validator">The typeof of the validator to use.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <exception cref="NullReferenceException"></exception>
        /// <exception cref="ValidationException"></exception>
        public static Task ValidateAsync(this object obj, Type validator, CancellationToken cancellationToken = default)
        {
            Validate(obj, validator);
            cancellationToken.ThrowIfCancellationRequested();

            return Task.CompletedTask;
        }


        // *** IENUMERABLE ***

        /// <summary>
        /// Executes the specified validator for all objects.
        /// </summary>
        /// <typeparam name="TValidator">The type of validator to use.</typeparam>
        /// <param name="objects">The objects to validate.</param>
        /// <exception cref="NullReferenceException"></exception>
        /// <exception cref="ValidationException"></exception>
        public static void Validate<TValidator>(this IEnumerable<object> objects)
            where TValidator : IValidator
            => Validate(objects, typeof(TValidator));

        /// <summary>
        /// Finds an appropiate validator and executes it for all objects.
        /// </summary>
        /// <param name="objects">The objects to validate.</param>
        /// <exception cref="NullReferenceException"></exception>
        /// <exception cref="ValidationException"></exception>
        public static void Validate(this IEnumerable<object> objects)
        {
            if (!objects.Any())
                return;

            var grps = objects.GroupBy(x => x.GetType());
            foreach (var grp in grps)
            {
                var validatorType = FindValidator(grp.Key);
                if (validatorType is null)
                    throw new NullReferenceException($"Validator for {grp.Key.Name} not found");

                Validate(grp.Select(i => i), validatorType);
            }
        }

        /// <summary>
        /// Executes the validator for all objects.
        /// </summary>
        /// <param name="objects">The object to validate.</param>
        /// <typeparam name="validator">The type of validator to use.</typeparam>
        /// <exception cref="NullReferenceException"></exception>
        /// <exception cref="ValidationException"></exception>
        public static void Validate<T>(this IEnumerable<T> objects, Type validator)
        {
            if (!objects.Any())
                return;

            var method = validator
                            .GetMethods()
                            .Where(m => m.Name.Equals("Validate", StringComparison.OrdinalIgnoreCase))
                            .First();

            var v = Activator.CreateInstance(validator);
            foreach (T obj in objects)
            {
                if (obj is null)
                    throw new NullReferenceException();

                ValidationResult? result = method.Invoke(v, new object[] { obj }) as ValidationResult;
                if (result is null || !result.IsValid)
                    throw new ValidationException(result?.Errors ?? new List<ValidationFailure>());
            }
        }


        /// <summary>
        /// Executes the specified validator for all objects.
        /// </summary>
        /// <typeparam name="TValidator">The type of validator to use.</typeparam>
        /// <param name="objects">The objects to validate.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <exception cref="NullReferenceException"></exception>
        /// <exception cref="ValidationException"></exception>
        public static Task ValidateAsync<TValidator>(this IEnumerable<object> objects, CancellationToken cancellationToken = default)
            where TValidator : IValidator

        {
            Validate<TValidator>(objects);
            cancellationToken.ThrowIfCancellationRequested();

            return Task.CompletedTask;

        }

        /// <summary>
        /// Finds an appropiate validator and executes it for all objects.
        /// </summary>
        /// <param name="objects">The objects to validate.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <exception cref="NullReferenceException"></exception>
        /// <exception cref="ValidationException"></exception>
        public static Task ValidateAsync(this IEnumerable<object> objects, CancellationToken cancellationToken = default)
        {
            Validate(objects);
            cancellationToken.ThrowIfCancellationRequested();

            return Task.CompletedTask;
        }

        /// <summary>
        /// Executes the validator for all objects.
        /// </summary>
        /// <typeparam name="validator">The type of validator to use.</typeparam>
        /// <param name="objects">The object to validate.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <exception cref="NullReferenceException"></exception>
        /// <exception cref="ValidationException"></exception>
        public static Task ValidateAsync<T>(this IEnumerable<T> objects, Type validator, CancellationToken cancellationToken = default)
        {
            Validate<T>(objects, validator);
            cancellationToken.ThrowIfCancellationRequested();

            return Task.CompletedTask;
        }
    }
}