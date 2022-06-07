using System;
using System.Threading;
using System.Threading.Tasks;

namespace Promitor.Agents.Scraper.Scheduling;

/// <summary>
/// Limits the number of threads for scraping tasks that can access the thread pool concurrently.
/// </summary>
public interface IScrapingMutex
{
    /// <summary>
    /// The current count of the <see cref="IScrapingMutex"/>.
    /// </summary>
    int CurrentCount { get; }

    /// <summary>
    /// Returns a <see cref="System.Threading.WaitHandle"/> that can be used to wait on the <see cref="IScrapingMutex"/>.
    /// </summary>
    WaitHandle AvailableWaitHandle { get; }

    /// <summary>
    /// Blocks the current thread until it can enter the <see cref="IScrapingMutex"/>.
    /// </summary>
    void Wait();

    /// <summary>
    /// Blocks the current thread until it can enter the <see cref="IScrapingMutex"/>,
    /// while observing a <see cref="System.Threading.CancellationToken"/>.
    /// </summary>
    /// <param name="cancellationToken">
    /// The <see cref="System.Threading.CancellationToken"/> token to observe.
    /// </param>
    void Wait(CancellationToken cancellationToken);

    /// <summary>
    /// Blocks the current thread until it can enter the <see cref="IScrapingMutex"/>,
    /// using a <see cref="System.TimeSpan"/> to measure the time interval.
    /// </summary>
    /// <param name="timeout">
    /// A <see cref="System.TimeSpan"/> that represents the number of milliseconds to wait,
    /// or a <see cref="System.TimeSpan"/> that represents -1 milliseconds to wait indefinitely.
    /// </param>
    /// <returns>
    /// true if the current thread successfully entered the <see cref="IScrapingMutex"/>; otherwise, false.
    /// </returns>
    bool Wait(TimeSpan timeout);
    
    /// <summary>
    /// Blocks the current thread until it can enter the <see cref="IScrapingMutex"/>,
    /// using a <see cref="System.TimeSpan"/> to measure the time interval, while observing a
    /// <see cref="System.Threading.CancellationToken"/>.
    /// </summary>
    /// <param name="timeout">
    /// A <see cref="System.TimeSpan"/> that represents the number of milliseconds to wait,
    /// or a <see cref="System.TimeSpan"/> that represents -1 milliseconds to wait indefinitely.
    /// </param>
    /// <param name="cancellationToken">
    /// The <see cref="System.Threading.CancellationToken"/> to observe.
    /// </param>
    /// <returns>
    /// true if the current thread successfully entered the <see cref="IScrapingMutex"/>; otherwise, false.
    /// </returns>
    bool Wait(TimeSpan timeout, CancellationToken cancellationToken);
    
    /// <summary>
    /// Blocks the current thread until it can enter the <see cref="IScrapingMutex"/>,
    /// using a 32-bit signed integer to measure the time interval.
    /// </summary>
    /// <param name="millisecondsTimeout">
    /// The number of milliseconds to wait, or <see cref="Timeout.Infinite"/>(-1) to wait indefinitely.
    /// </param>
    /// <returns>
    /// true if the current thread successfully entered the <see cref="IScrapingMutex"/>; otherwise, false.
    /// </returns>
    bool Wait(int millisecondsTimeout);

    /// <summary>
    /// Blocks the current thread until it can enter the <see cref="IScrapingMutex"/>,
    /// using a 32-bit signed integer to measure the time interval,
    /// while observing a <see cref="System.Threading.CancellationToken"/>.
    /// </summary>
    /// <param name="millisecondsTimeout">
    /// The number of milliseconds to wait, or <see cref="Timeout.Infinite"/>(-1) to wait indefinitely.
    /// </param>
    /// <param name="cancellationToken">
    /// The <see cref="System.Threading.CancellationToken"/> to observe.
    /// </param>
    /// <returns>
    /// true if the current thread successfully entered the <see cref="IScrapingMutex"/>; otherwise, false.
    /// </returns>
    bool Wait(int millisecondsTimeout, CancellationToken cancellationToken);

    /// <summary>
    /// Asynchronously waits to enter the <see cref="IScrapingMutex"/>.
    /// </summary>
    /// <returns>A task that will complete when the semaphore has been entered.</returns>
    Task WaitAsync();

    /// <summary>
    /// Asynchronously waits to enter the <see cref="IScrapingMutex"/>, while observing a
    /// <see cref="System.Threading.CancellationToken"/>.
    /// </summary>
    /// <returns>A task that will complete when the semaphore has been entered.</returns>
    /// <param name="cancellationToken">
    /// The <see cref="System.Threading.CancellationToken"/> token to observe.
    /// </param>
    Task WaitAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Asynchronously waits to enter the <see cref="IScrapingMutex"/>,
    /// using a 32-bit signed integer to measure the time interval.
    /// </summary>
    /// <param name="millisecondsTimeout">
    /// The number of milliseconds to wait, or <see cref="Timeout.Infinite"/>(-1) to wait indefinitely.
    /// </param>
    /// <returns>
    /// A task that will complete with a result of true if the current thread successfully entered
    /// the <see cref="IScrapingMutex"/>, otherwise with a result of false.
    /// </returns>
    Task<bool> WaitAsync(int millisecondsTimeout);

    /// <summary>
    /// Asynchronously waits to enter the <see cref="IScrapingMutex"/>,
    /// using a <see cref="System.TimeSpan"/> to measure the time interval, while observing a
    /// <see cref="System.Threading.CancellationToken"/>.
    /// </summary>
    /// <param name="timeout">
    /// A <see cref="System.TimeSpan"/> that represents the number of milliseconds
    /// to wait, or a <see cref="System.TimeSpan"/> that represents -1 milliseconds to wait indefinitely.
    /// </param>
    /// <returns>
    /// A task that will complete with a result of true if the current thread successfully entered
    /// the <see cref="IScrapingMutex"/>, otherwise with a result of false.
    /// </returns>
    Task<bool> WaitAsync(TimeSpan timeout);

    /// <summary>
    /// Asynchronously waits to enter the <see cref="IScrapingMutex"/>,
    /// using a <see cref="System.TimeSpan"/> to measure the time interval.
    /// </summary>
    /// <param name="timeout">
    /// A <see cref="System.TimeSpan"/> that represents the number of milliseconds
    /// to wait, or a <see cref="System.TimeSpan"/> that represents -1 milliseconds to wait indefinitely.
    /// </param>
    /// <param name="cancellationToken">
    /// The <see cref="System.Threading.CancellationToken"/> token to observe.
    /// </param>
    /// <returns>
    /// A task that will complete with a result of true if the current thread successfully entered
    /// the <see cref="IScrapingMutex"/>, otherwise with a result of false.
    /// </returns>
    Task<bool> WaitAsync(TimeSpan timeout, CancellationToken cancellationToken);

    /// <summary>
    /// Asynchronously waits to enter the <see cref="IScrapingMutex"/>,
    /// using a 32-bit signed integer to measure the time interval,
    /// while observing a <see cref="System.Threading.CancellationToken"/>.
    /// </summary>
    /// <param name="millisecondsTimeout">
    /// The number of milliseconds to wait, or <see cref="Timeout.Infinite"/>(-1) to wait indefinitely.
    /// </param>
    /// <param name="cancellationToken">The <see cref="System.Threading.CancellationToken"/> to observe.</param>
    /// <returns>
    /// A task that will complete with a result of true if the current thread successfully entered
    /// the <see cref="IScrapingMutex"/>, otherwise with a result of false.
    /// </returns>
    Task<bool> WaitAsync(int millisecondsTimeout, CancellationToken cancellationToken);

    /// <summary>
    /// Exits the <see cref="IScrapingMutex"/> once.
    /// </summary>
    /// <returns>The previous count of the <see cref="IScrapingMutex"/>.</returns>
    int Release();

    /// <summary>
    /// Exits the <see cref="IScrapingMutex"/> a specified number of times.
    /// </summary>
    /// <param name="releaseCount">The number of times to exit the semaphore.</param>
    /// <returns>The previous count of the <see cref="IScrapingMutex"/>.</returns>
    int Release(int releaseCount);
}