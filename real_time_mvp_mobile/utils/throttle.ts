// @ts-nocheck

export default function throttle(mainFunc: (args: any) => void, delay: number) {
    let timerFlag: NodeJS.Timeout | null = null;
    return (...args: any) => {
        if (!timerFlag) {
            mainFunc(...args);
            timerFlag = setTimeout(() => {
                timerFlag = null
            }, delay)
        }
    }
}