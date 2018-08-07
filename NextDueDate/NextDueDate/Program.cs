using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using NextDueDate.Models;
using Newtonsoft.Json;
using System.Linq;
using System.Collections.Generic;

namespace NextDueDate
{
    class Program
    {
        public static void Main()
        {
            int inputCount = 0;
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "rpc_queue", durable: false,
                  exclusive: false, autoDelete: false, arguments: null);
                channel.BasicQos(0, 1, false);
                var consumer = new EventingBasicConsumer(channel);
                channel.BasicConsume(queue: "rpc_queue",
                  autoAck: false, consumer: consumer);
                Console.WriteLine(" [x] Awaiting RPC requests");

                consumer.Received += (model, ea) =>
                {
                    string response = null;

                    var body = ea.Body;
                    var props = ea.BasicProperties;
                    var replyProps = channel.CreateBasicProperties();
                    replyProps.CorrelationId = props.CorrelationId;

                    try
                    {
                        inputCount++;

                        Aircraft data = new Aircraft();

                        Aircraft[] sample = data.GetSampleData();


                        var message = Encoding.UTF8.GetString(body);

                        IO inputs = JsonConvert.DeserializeObject<IO>(message);

                        if(inputs.fDTasks.Count > 0){
                            List<FDTask> outputs = new List<FDTask>();
                            int index = 0;

                            int x = inputs.aircraftID - 1;

                            Aircraft test = sample[x];

                            foreach (FDTask task in inputs.fDTasks){
                                outputs.Add(NextDueDate(task, test));
                                index++;
                            }

                            IO outputObject = new IO();
                            outputObject.aircraftID = inputs.aircraftID;
                            outputObject.fDTasks = outputs;
                            response = JsonConvert.SerializeObject(outputObject);
                            Console.WriteLine(response);
                        }
                        else{
                            Console.WriteLine("Input length is 0");
                            response = "Input length is 0";
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(" [.] " + e.Message);
                        response = "";
                    }
                    finally
                    {
                        var responseBytes = Encoding.UTF8.GetBytes(response);
                        channel.BasicPublish(exchange: "", routingKey: props.ReplyTo,
                          basicProperties: replyProps, body: responseBytes);
                        channel.BasicAck(deliveryTag: ea.DeliveryTag,
                          multiple: false);
                    }
                };

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
        }

        /// 

        /// Assumes only valid positive integer input.
        /// Don't expect this one to work for big numbers, and it's
        /// probably the slowest recursive implementation possible.
        /// 
        private static FDTask NextDueDate(FDTask input, Aircraft plane)
        {
            DateTime today = new DateTime(2018, 6, 19);
            DateTime logDate;
            DateTime intervalMonthsNextDueDate = new DateTime();
            //string intervalMonthsNextDueDate;

            //f1
            //sif(String.IsNullOrEmpty(input.logDate) == true || input.intervalMonths == null){
                //intervalMonthsNextDueDate = null;
            //}
            //selse{
                logDate = Convert.ToDateTime(input.logDate);
                int addMonths = input.intervalMonths.GetValueOrDefault();
                logDate = logDate.AddMonths(addMonths);
                intervalMonthsNextDueDate = logDate;
            //}

            double daysRemainingByHoursInterval;
            int logHours = input.logHours.GetValueOrDefault();
            int intervalHours = input.intervalHours.GetValueOrDefault();
               
            //if(input.logHours == null || input.lo)

            //f2
            daysRemainingByHoursInterval = ((logHours + intervalHours) - plane.currentHours ) / plane.dailyHours;

            //f3
            DateTime hoursIntervalNextDueDate = today.AddHours(daysRemainingByHoursInterval);

            //f4
            if(intervalMonthsNextDueDate == null && hoursIntervalNextDueDate == null){
                input.nextDue = null;
            }
            else{
                int dif = DateTime.Compare(intervalMonthsNextDueDate, hoursIntervalNextDueDate);
                if (dif < 0)
                {
                    input.nextDue = intervalMonthsNextDueDate;
                }
                if (dif == 0)
                {
                    input.nextDue = intervalMonthsNextDueDate;
                }
                if (dif > 0)
                {
                    input.nextDue = hoursIntervalNextDueDate;
                }
            }

            return input;

            //return count + ": " + input;
        }
    }
}
