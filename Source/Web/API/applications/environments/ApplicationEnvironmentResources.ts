/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { field } from '@aksio/fundamentals';

import { AzureResources } from './AzureResources';
import { MongoDBResource } from './MongoDBResource';

export class ApplicationEnvironmentResources {

    @field(String)
    id!: string;

    @field(AzureResources)
    azure!: AzureResources;

    @field(MongoDBResource)
    mongoDB!: MongoDBResource;
}
