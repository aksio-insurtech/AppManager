/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { Command } from '@aksio/cratis-applications-frontend/commands';

export class CreateApplication extends Command {
    readonly route: string = '/api/applications';

    applicationId!: string;
    name!: string;
}
