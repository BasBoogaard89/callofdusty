import { HumanizeColumnPipe } from './humanize-column.pipe';

describe('HumanizeColumnPipe', () => {
    it('create an instance', () => {
        const pipe = new HumanizeColumnPipe();
        expect(pipe).toBeTruthy();
    });
});
