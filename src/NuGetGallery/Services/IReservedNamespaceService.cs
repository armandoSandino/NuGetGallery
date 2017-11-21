﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Threading.Tasks;

namespace NuGetGallery
{
    public interface IReservedNamespaceService
    {
        /// <summary>
        /// Create a new namespace with the given prefix
        /// </summary>
        /// <param name="prefix">The reserved namespace to be created</param>
        /// <returns>Awaitable Task</returns>
        Task AddReservedNamespaceAsync(ReservedNamespace prefix);

        /// <summary>
        /// Deallocate the reserved namespace with the given prefix, also removes
        /// the verified property on all the package registrations which match
        /// this reserved namespace only.
        /// </summary>
        /// <param name="prefix">The reserved namespace to be deleted</param>
        /// <returns>Awaitable Task</returns>
        Task DeleteReservedNamespaceAsync(string prefix);

        /// <summary>
        /// Adds the specified user as an owner to the reserved namespace.
        /// Also, all the package registrations owned by this user which match the 
        /// specified namespace will be marked as verified.
        /// </summary>
        /// <param name="prefix">The reserved namespace to modify</param>
        /// <param name="username">The user who gets ownership of the namespace</param>
        /// <returns>Awaitable Task</returns>
        Task AddOwnerToReservedNamespaceAsync(string prefix, string username);

        /// <summary>
        /// Remove the specified user as an owner from the reserved namespace.
        /// Also, all the package registrations owned by this user which match the 
        /// specified namespace only will be marked as unverifed.
        /// </summary>
        /// <param name="prefix">The reserved namespace to modify</param>
        /// <param name="username">The user to remove the ownership for the namespace</param>
        /// <param name="commitAsTransaction">True if the changes will be commited as a single transaction.</param>
        /// <returns>Awaitable Task</returns>
        Task DeleteOwnerFromReservedNamespaceAsync(string prefix, string username, bool commitAsTransaction);

        /// <summary>
        /// Add the specified package registration to the reserved namespace. It is the caller's reponsibility to
        /// commit the changes to the database.
        /// </summary>
        /// <param name="prefix">The reserved namespace to modify</param>
        /// <param name="packageRegistration">The package registration to be added</param>
        void AddPackageRegistrationToNamespace(string prefix, PackageRegistration packageRegistration);

        /// <summary>
        /// Remove the specified package registration from the reserved namespace. It is the caller's reponsibility to
        /// commit the changes to the database.
        /// </summary>
        /// <param name="prefix">The prefix value of the reserved namespace to modify</param>
        /// <param name="packageRegistration">The package registration entity to be removed.</param>
        void RemovePackageRegistrationFromNamespace(string prefix, PackageRegistration packageRegistration);

        /// <summary>
        /// Retrieves the first reserved namespace which matches the given prefix.
        /// </summary>
        /// <param name="prefix">The prefix to lookup</param>
        /// <returns>Reserved namespace matching the prefix</returns>
        ReservedNamespace FindReservedNamespaceForPrefix(string prefix);

        /// <summary>
        /// Retrieves all the reserved namespaces which matches the given prefix.
        /// </summary>
        /// <param name="prefix">The prefix to lookup</param>
        /// <returns>The list of reserved namespaces matching the prefix</returns>
        IReadOnlyCollection<ReservedNamespace> FindAllReservedNamespacesForPrefix(string prefix, bool getExactMatches);

        /// <summary>
        /// Retrieves all the reserved namespaces which matches the given list of prefixes.
        /// </summary>
        /// <param name="prefixList">The list of prefixes to lookup</param>
        /// <returns>The list of reserved namespaces matching the prefixes</returns>
        IReadOnlyCollection<ReservedNamespace> FindReservedNamespacesForPrefixList(IReadOnlyCollection<string> prefixList);

        /// <summary>
        /// Retrieves all the reserved namespaces which match the given id
        /// </summary>
        /// <param name="id">The package id to lookup</param>
        /// <returns>The list of reserved namespaces which are prefixes for the given id</returns>
        IReadOnlyCollection<ReservedNamespace> GetReservedNamespacesForId(string id);

        /// <summary>
        /// Verifies if the id is allowed to be pushed by the user or not.
        /// </summary>
        /// <param name="id">The package id to lookup</param>
        /// <param name="user">The user to verify for permission to push to new id</param>
        /// <param name="userOwnedMatchingNamespaces">The out list of namespaces owned by the user that match <paramref name="id"/></param>
        /// <returns>True if the push is allowed for the specified user for the given id, false otherwise</returns>
        bool IsPushAllowed(string id, User user, out IReadOnlyCollection<ReservedNamespace> userOwnedMatchingNamespaces);

        /// <summary>
        /// Verifies if the id is allowed to be pushed by the user or any users that the user can push on behalf of.
        /// </summary>
        /// <param name="id">The package id to lookup</param>
        /// <param name="user">The user to verify for permission to push to new id</param>
        /// <param name="userOwnedMatchingNamespaces">The out list of namespaces owned by the user that match <paramref name="id"/></param>
        /// <returns>True if the push is allowed for the specified user or any users that the user can push on behalf of for the given id, false otherwise</returns>
        bool IsPushAllowedOnBehalfOfOwners(string id, User user, out IReadOnlyCollection<ReservedNamespace> userOwnedMatchingNamespaces);
    }
}