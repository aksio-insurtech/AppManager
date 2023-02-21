/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { field } from '@aksio/cratis-fundamentals';

import { ApplicationEnvironmentConsolidationKey } from './ApplicationEnvironmentConsolidationKey';
import { ApplicationEnvironmentConsolidationStatus } from './ApplicationEnvironmentConsolidationStatus';

export class ApplicationEnvironmentConsolidation {

    @field(ApplicationEnvironmentConsolidationKey)
    id!: ApplicationEnvironmentConsolidationKey;

    @field(Date)
    started!: Date;

    @field(Date)
    completedOrFailed!: Date;

    @field(Number)
    status!: ApplicationEnvironmentConsolidationStatus;

    @field(String)
    log!: string;
}
