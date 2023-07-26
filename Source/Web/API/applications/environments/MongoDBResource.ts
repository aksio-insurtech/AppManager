/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { field } from '@aksio/fundamentals';

import { MongoDBUser } from './MongoDBUser';

export class MongoDBResource {

    @field(String)
    connectionString!: string;

    @field(MongoDBUser, true)
    users!: MongoDBUser[];
}
