/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { field } from '@aksio/cratis-fundamentals';

import { AzureResources } from './AzureResources';
import { MongoDBResource } from './MongoDBResource';

export class ApplicationResources {

    @field(AzureResources)
    azure!: AzureResources;

    @field(MongoDBResource)
    mongoDB!: MongoDBResource;
}
